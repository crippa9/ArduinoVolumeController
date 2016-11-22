using Microsoft.Win32;
using System;
using System.Security.Principal;

namespace Volumecontroller {
    public class StartUpManager {
        static string appName = "VolumeController";
        public static void AddApplicationToCurrentUserStartup() {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true)) {
                key.SetValue(appName, "\"" + System.Reflection.Assembly.GetExecutingAssembly().Location + "\"");
            }
        }

        public static void AddApplicationToAllUserStartup() {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true)) {
                key.SetValue(appName, "\"" + System.Reflection.Assembly.GetExecutingAssembly().Location + "\"");
            }
        }

        public static void RemoveApplicationFromCurrentUserStartup() {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true)) {
                key.DeleteValue(appName, false);
            }
        }

        public static void RemoveApplicationFromAllUserStartup() {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true)) {
                key.DeleteValue(appName, false);
            }
        }

        public static bool IsUserAdministrator() {
            //bool value to hold our return value
            bool isAdmin;
            try {
                //get the currently logged in user
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (UnauthorizedAccessException ex) {
                isAdmin = false;
            }
            catch (Exception ex) {
                isAdmin = false;
            }
            return isAdmin;
        }


    }


}
