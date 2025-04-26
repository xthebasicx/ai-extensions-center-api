require 'sketchup.rb'
require 'win32ole'
require 'net/http'
require 'json'
require 'uri'
require 'openssl'
require 'base64'

class AIExtensionsCenter
  attr_reader :check_license

  def initialize(extension_id)
    @extension_id = extension_id
    @license_Key = nil
    @check_license = false
    @hwid = nil
    @license_file = File.join(File.dirname(__FILE__), "license.json")
    @secret_key = 'c17e4a89f3b24d0d9aa1e2c6b548fe12'
    @aes_algo = 'AES-256-CBC'
  end

  def encrypt(text)
    cipher = OpenSSL::Cipher.new(@aes_algo)
    cipher.encrypt
    iv = cipher.random_iv
    cipher.key = @secret_key
    encrypted = cipher.update(text) + cipher.final
    Base64.strict_encode64(iv + encrypted)
  end

  def decrypt(cipher_text)
    decoded = Base64.strict_decode64(cipher_text)
    iv = decoded[0..15]
    encrypted_data = decoded[16..-1]
    decipher = OpenSSL::Cipher.new(@aes_algo)
    decipher.decrypt
    decipher.key = @secret_key
    decipher.iv = iv
    decipher.update(encrypted_data) + decipher.final
  end

  def get_hwid
    cpu = WIN32OLE.connect("winmgmts://").ExecQuery("SELECT ProcessorId FROM Win32_Processor").each.first
    @hwid = cpu ? cpu.ProcessorId : "Unknown"
  end

  def validate_license
    get_hwid()
    if File.exist?(@license_file)
      data = JSON.parse(File.read(@license_file))
      @license_Key = data["licenseKey"]
    else
      return
    end
    validate_license_api(@extension_id, @license_Key, @hwid)
  end

  def show_license_form
    html = license_form
    dlg = dialog(html)
    dlg.show
  end

  def dialog(html)
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
      activate_license_api(@extension_id, license_key, email, @hwid)
    end
    dlg
  end

  def license_form
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

  def activate_license_api(extension_id, license_Key, email, hwid)
    encrypted_license = encrypt(license_Key)
    url = "https://ai.extensions.center.api.sketchunivercity.com/api/Licenses/activate"
    request = Sketchup::Http::Request.new(url)
    request.method = Sketchup::Http::POST
    request.headers = {"Content-Type" => "application/json"}
    request.body = {extensionId: extension_id, licenseKey: encrypted_license, email: email, hwId: hwid}.to_json

    request.start do |req, res|
      if res.status_code == 200
        body = JSON.parse(res.body)
        encrypted_result = body["result"]
        result = decrypt(encrypted_result)
        if result == "Success"
          File.write(@license_file, {licenseKey: encrypted_license}.to_json)
          UI.messagebox('Activation successful')
          validate_license_api(extension_id, encrypted_license, hwid)
        else
          UI.messagebox('Activation failed.')
        end
      else
        UI.messagebox('Activation failed.')
      end
    end
  end

  def validate_license_api(extension_id, encrypted_license, hwid)
    url = "https://ai.extensions.center.api.sketchunivercity.com/api/Licenses/validate"
    request = Sketchup::Http::Request.new(url)
    request.method = Sketchup::Http::POST
    request.headers = {"Content-Type" => "application/json"}
    request.body = {extensionId: extension_id, licenseKey: encrypted_license, hwId: hwid}.to_json
    
    request.start do |req, res|
      if res.status_code == 200
        body = JSON.parse(res.body)
        encrypted_result = body["result"]
        result = decrypt(encrypted_result)
          if result == "Success"
            @check_license = true
          else
            @check_license = false
          end
      else
        @check_license = false
      end
    end
  end
end
