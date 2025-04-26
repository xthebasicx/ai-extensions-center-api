Sketchup.require File.join(File.dirname(__FILE__), 'AIExtensionsCenter')

module {{ModuleName}}
  AIExtensionsCenter = AIExtensionsCenter.new("{{EXTENSION_ID}}")

  class Observer < Sketchup::AppObserver
    def onExtensionsLoaded
      AIExtensionsCenter.validate_license
    end
  end

  Sketchup.add_observer(Observer.new)
end