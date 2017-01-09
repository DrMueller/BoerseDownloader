namespace MMU.BoerseDownloader.Integration.Infrastructure
{
    internal static class Constants
    {
        public const string DOWNLOADENTRYTITLE_SEASONPACK = "Staffel-Pack";
        
        internal static class BoerseUserControls
        {
            internal const string TEXTBOX_LOGIN = "ctrl_pageLogin_login";
            internal const string TEXTBOX_PASSWORD = "ctrl_pageLogin_password";

            internal const string CHECKBOX_REMEMBER_LOGIN = "ctrl_pageLogin_remember";


        }
    }
}



//var loginCtrl = (IHTMLInputElement)htmlElements.FirstOrDefault(f => f.id == "LoginControl");
//            if (loginCtrl == null)
//            {
//                return false;
//            }

//            var passwordControl = (IHTMLInputElement)htmlElements.First(f => f.id == "ctrl_password");
//var keepBeingLoggedInCtrl = (IHTMLInputElement)htmlElements.First(f => f.id == "ctrl_remember");

//keepBeingLoggedInCtrl.@checked = true;
//            loginCtrl.value = boerseUser.LoginName;
//            passwordControl.value = boerseUser.Password;

//            return true;
//        }

//        private static void OpenLoginForm(IHTMLDocument2 doc)
//{
//    var allElements = doc.all.Cast<IHTMLElement>();
//    var loginButton = allElements.FirstOrDefault(f => f.id == "SignupButton");