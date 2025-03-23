require 'sketchup.rb'
require 'win32ole'
require 'net/http'
require 'json'
require 'uri'

module SketchupHome
  module AiExtensionsCenter
    PLUGIN_DIR = File.dirname(__FILE__)
    LICENSE_FILE = File.join(PLUGIN_DIR, "license_data.json")
    
    @extension_id = "{{EXTENSION_ID}}"
    @license_Key = nil
    @validate_license = false
    @hwid = nil

    # get hwid
    def self.get_hwid
      cpu = WIN32OLE.connect("winmgmts://").ExecQuery("SELECT ProcessorId FROM Win32_Processor").each.first
      @hwid = cpu ? cpu.ProcessorId : "Unknown"
    end

    # validate license file
    def self.validate_license
      get_hwid()
      if File.exist?(LICENSE_FILE)
        data = JSON.parse(File.read(LICENSE_FILE))
        @license_Key = data["licenseKey"]
      else
        UI.messagebox("License file not found.")
        return
      end
      validate_license_api(@extension_id, @license_Key, @hwid)
    end
    
    # main license fucntion
    def self.license
      html = license_form
      dlg = dialog(html)
      dlg.show
    end
    
    # dialog
    def self.dialog(html)
      dlg = UI::HtmlDialog.new({
        :dialog_title => "License",
        :width => 250,
        :height => 150,
        :min_width => 250,
        :min_height => 150,
        :resizable => true,
        :scrollable => true,
        :style => UI::HtmlDialog::STYLE_DIALOG
      })
      dlg.set_html(html)
      dlg.add_action_callback("activate_license") do |action_context, license_key, email|
        get_hwid()
        activate_license_api(license_key, email, @hwid)
      end
      dlg
    end

    # html
    def self.license_form
      html = <<-HTML
        <html>
          <head>
            <script type="text/javascript">
              function submitForm() {
                var email = document.getElementById("email").value;
                var licenseKey = document.getElementById("licenseKey").value;
                if (window.sketchup) {
                  window.sketchup.activate_license(licenseKey, email);
                } else {
                  alert("SketchUp API not available!");
                }
              }
              </script>
          </head>
          <body>
            <form onsubmit="event.preventDefault(); submitForm();">
              <label for="email">Email</label><br>
              <input type="email" id="email" name="email" placeholder="Enter email" style="width: 100%;"/><br>
              <label for="licenseKey">License Key</label><br>
              <input type="text" id="licenseKey" name="licenseKey" placeholder="Enter license key" style="width: 100%;"/><br>
              <button type="submit">Submit</button>
            </form>
          </body>
        </html>
      HTML
      html
    end

    # license api
    def self.activate_license_api(license_Key, email, hwid)
      url = "http://localhost:5000/api/Licenses/activate"
      request = Sketchup::Http::Request.new(url)
      request.method = Sketchup::Http::POST
      request.headers = {"Content-Type" => "application/json"}
      request.body = {licenseKey: license_Key, email: email, hwId: hwid}.to_json

      request.start do |req, res|
        if res.status_code == 200
          File.write(LICENSE_FILE, {licenseKey: license_Key}.to_json)
          UI.messagebox('Activation successful and license saved.')
        else
          UI.messagebox('Activation failed.')
        end
      end
    end

    # validate license api
    def self.validate_license_api(extension_id, license_Key, hwid)
      url = "http://localhost:5000/api/Licenses/validate"
      request = Sketchup::Http::Request.new(url)
      request.method = Sketchup::Http::POST
      request.headers = {"Content-Type" => "application/json"}
      request.body = {extensionId: extension_id, licenseKey: license_Key, hwId: hwid}.to_json
      
      request.start do |req, res|
        if res.status_code == 200
          UI.messagebox('License validation successful.')
          @validate_license = true
        else
          UI.messagebox('License validation failed.')
          @validate_license = false
        end
      end
    end

  end
end
