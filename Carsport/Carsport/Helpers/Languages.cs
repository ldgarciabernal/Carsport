namespace Carsport.Helpers
{
    using Xamarin.Forms;
    using Interfaces;
    using Resources;

    public static class Languages
    {
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }
        
        public static string Error
        {
            get { return Resource.Error; }
        }

        public static string CarButton
        {
            get { return Resource.CarButton; }
        }

        public static string BusButton
        {
            get { return Resource.BusButton; }
        }

        public static string TrainButton
        {
            get { return Resource.TrainButton; }
        }

        public static string BikeButton
        {
            get { return Resource.BikeButton; }
        }

        public static string DashBoardTitle
        {
            get { return Resource.DashBoardTitle; }
        }

        public static string AcceptDisplay
        {
            get { return Resource.AcceptDisplay; }
        }

        public static string NoInternetConnection
        {
            get { return Resource.NoInternetConnection; }
        }
        
        public static string CommuneOriginValidation
        {
            get { return Resource.CommuneOriginValidation; }
        }


        public static string CommuneDestinyValidation
        {
            get { return Resource.CommuneDestinyValidation; }
        }


        public static string CoreOriginValidation
        {
            get { return Resource.CoreOriginValidation; }
        }

        public static string CoreDestinyValidation
        {
            get { return Resource.CoreDestinyValidation; }
        }

        public static string InternetError
        {
            get { return Resource.InternetError; }
        }

        public static string OkMessage
        {
            get { return Resource.OkMessage; }
        }

        public static string SwitchButton
        {
            get { return Resource.SwitchButton; }
        }

        public static string StopButton
        {
            get { return Resource.StopButton; }
        }

        public static string BusTitle
        {
            get { return Resource.BusTitle; }
        }

        public static string LabelCommuneOrigin
        {
            get { return Resource.LabelCommuneOrigin; }
        }

        public static string PickerCommuneOrigin
        {
            get { return Resource.PickerCommuneOrigin; }
        }

        public static string LabelCoreOrigin
        {
            get { return Resource.LabelCoreOrigin; }
        }

        public static string PickerCoreOrigin
        {
            get { return Resource.PickerCoreOrigin; }
        }

        public static string LabelCommuneDestiny
        {
            get { return Resource.LabelCommuneDestiny; }
        }

        public static string PickerCommuneDestiny
        {
            get { return Resource.PickerCommuneDestiny; }
        }

        public static string LabelCoreDestiny
        {
            get { return Resource.LabelCoreDestiny; }
        }

        public static string PickerCoreDestiny
        {
            get { return Resource.PickerCoreDestiny; }
        }

        public static string SearchButton
        {
            get { return Resource.SearchButton; }
        }

        public static string SheduleTitle
        {
            get { return Resource.SheduleTitle; }
        }

        public static string PickerLines
        {
            get { return Resource.PickerLines; }
        }

        public static string StopTittle
        {
            get { return Resource.StopTittle; }
        }

        public static string NotShedule
        {
            get { return Resource.NotShedule; }
        }

        public static string TrainTitle
        {
            get { return Resource.TrainTitle; }
        }

        public static string LabelStationOrigin
        {
            get { return Resource.LabelStationOrigin; }
        }

        public static string LabelStationDestiny
        {
            get { return Resource.LabelStationDestiny; }
        }

        public static string LabelStationPickerOrigin
        {
            get { return Resource.LabelStationPickerOrigin; }
        }

        public static string LabelStationPickerDestiny
        {
            get { return Resource.LabelStationPickerDestiny; }
        }

        public static string OriginStationValidation
        {
            get { return Resource.OriginStationValidation; }
        }

        public static string DestinyStationValidation
        {
            get { return Resource.DestinyStationValidation; }
        }
        
        public static string EqualsStationValidation
        {
            get { return Resource.EqualsStationValidation; }
        }

        public static string TrainSheduleTitle
        {
            get { return Resource.TrainSheduleTitle; }
        }

        public static string StationTittle
        {
            get { return Resource.StationTittle; }
        }

        public static string StationButton
        {
            get { return Resource.StationButton; }
        }

        public static string BikeTitle
        {
            get { return Resource.BikeTitle; }
        }

        public static string PickerBycicles
        {
            get { return Resource.PickerBycicles; }
        }

        public static string EmailLoginLabel
        {
            get { return Resource.EmailLoginLabel; }
        }

        public static string EmailLoginPlaceHolder
        {
            get { return Resource.EmailLoginPlaceHolder; }
        }

        public static string PasswordLoginLabel
        {
            get { return Resource.PasswordLoginLabel; }
        }

        public static string PasswordLoginPlaceHolder
        {
            get { return Resource.PasswordLoginPlaceHolder; }
        }

        public static string KeepLogin
        {
            get { return Resource.KeepLogin; }
        }

        public static string ButtonLogin
        {
            get { return Resource.ButtonLogin; }
        }

        public static string RegisterLink
        {
            get { return Resource.RegisterLink; }
        }

        public static string ForgotPasswordLink
        {
            get { return Resource.ForgotPasswordLink; }
        }

        public static string EmailValidation
        {
            get { return Resource.EmailValidation; }
        }

        public static string PasswordValidation
        {
            get { return Resource.PasswordValidation; }
        }

        public static string GetTokenWrong
        {
            get { return Resource.GetTokenWrong; }
        }

        public static string CarTitle
        {
            get { return Resource.CarTitle; }
        }

        public static string ProfileTitle
        {
            get { return Resource.ProfileTitle; }
        }

        public static string LogOutTitle
        {
            get { return Resource.LogOutTitle; }
        }

        public static string ChangeImage
        {
            get { return Resource.ChangeImage; }
        }

        public static string RegisterFirstName
        {
            get { return Resource.RegisterFirstName; }
        }

        public static string RegisterFirstNamePlaceholder
        {
            get { return Resource.RegisterFirstNamePlaceholder; }
        }

        public static string RegisterLastName
        {
            get { return Resource.RegisterLastName; }
        }

        public static string RegisterLastNamePlaceholder
        {
            get { return Resource.RegisterLastNamePlaceholder; }
        }

        public static string RegisterEMail
        {
            get { return Resource.RegisterEMail; }
        }

        public static string RegisterEmailPlaceHolder
        {
            get { return Resource.RegisterEmailPlaceHolder; }
        }

        public static string RegisterPassword
        {
            get { return Resource.RegisterPassword; }
        }

        public static string RegisterPasswordPlaceHolder
        {
            get { return Resource.RegisterPasswordPlaceHolder; }
        }

        public static string RegisterPasswordConfirm
        {
            get { return Resource.RegisterPasswordConfirm; }
        }

        public static string RegisterPasswordConfirmPlaceHolder
        {
            get { return Resource.RegisterPasswordConfirmPlaceHolder; }
        }

        public static string RegisterSave
        {
            get { return Resource.RegisterSave; }
        }

        public static string RegisterFirstNameError
        {
            get { return Resource.RegisterFirstNameError; }
        }

        public static string RegisterLastNameError
        {
            get { return Resource.RegisterLastNameError; }
        }

        public static string RegisterEMailError
        {
            get { return Resource.RegisterEMailError; }
        }

        public static string RegisterPasswordError
        {
            get { return Resource.RegisterPasswordError; }
        }

        public static string RegisterPasswordConfirmError
        {
            get { return Resource.RegisterPasswordConfirmError; }
        }

        public static string RegisterPasswordsNoMatch
        {
            get { return Resource.RegisterPasswordsNoMatch; }
        }

        public static string ImageSource
        {
            get { return Resource.ImageSource; }
        }

        public static string FromGallery
        {
            get { return Resource.FromGallery; }
        }

        public static string NewPicture
        {
            get { return Resource.NewPicture; }
        }

        public static string Cancel
        {
            get { return Resource.Cancel; }
        }

        public static string ConfirmRegister
        {
            get { return Resource.ConfirmRegister; }
        }

        public static string RegisterConfirmation
        {
            get { return Resource.RegisterConfirmation; }
        }

        public static string NotificationTitle
        {
            get { return Resource.NotificationTitle; }
        }

        public static string NotificationSearchPlaceHolder
        {
            get { return Resource.NotificationSearchPlaceHolder; }
        }

        public static string ConfirmDeleteRoute
        {
            get { return Resource.ConfirmDeleteRoute; }
        }

        public static string DeleteRouteConfirmation
        {
            get { return Resource.DeleteRouteConfirmation; }
        }

        public static string Yes
        {
            get { return Resource.Yes; }
        }

        public static string No
        {
            get { return Resource.No; }
        }

        public static string RoutesTitle
        {
            get { return Resource.RoutesTitle; }
        }

        public static string SearchRoute
        {
            get { return Resource.SearchRoute; }
        }

        public static string Edit
        {
            get { return Resource.Edit; }
        }

        public static string Delete
        {
            get { return Resource.Delete; }
        }

        public static string NumOfSeatValidation
        {
            get { return Resource.NumOfSeatValidation; }
        }

        public static string OriginCityValidation
        {
            get { return Resource.OriginCityValidation; }
        }

        public static string DestinyCityValidation
        {
            get { return Resource.DestinyCityValidation; }
        }

        public static string DateValidation
        {
            get { return Resource.DateValidation; }
        }

        public static string TimeValidation
        {
            get { return Resource.TimeValidation; }
        }

        public static string NumOfSeatMinValidation
        {
            get { return Resource.NumOfSeatMinValidation; }
        }

        public static string CreateRouteTitle
        {
            get { return Resource.CreateRouteTitle; }
        }

        public static string ButtonCreateRoute
        {
            get { return Resource.ButtonCreateRoute; }
        }

        public static string LabelNumOfSeat
        {
            get { return Resource.LabelNumOfSeat; }
        }

        public static string LabelDate
        {
            get { return Resource.LabelDate; }
        }

        public static string LabelDestinyCity
        {
            get { return Resource.LabelDestinyCity; }
        }

        public static string LabelOriginCity
        {
            get { return Resource.LabelOriginCity; }
        }

        public static string PickerOriginCity
        {
            get { return Resource.PickerOriginCity; }
        }

        public static string PickerDestinyCity
        {
            get { return Resource.PickerDestinyCity; }
        }

        public static string SuccessfulEdit
        {
            get { return Resource.SuccessfulEdit; }
        }

        public static string EditMessages
        {
            get { return Resource.EditMessages; }
        }

        public static string LabelDescription
        {
            get { return Resource.LabelDescription; }
        }

        public static string ConfigurationTitle
        {
            get { return Resource.ConfigurationTitle; }
        }

        public static string ChatMenuTitle
        {
            get { return Resource.ChatMenuTitle; }
        }

        public static string MyRouteMenuTitle
        {
            get { return Resource.MyRouteMenuTitle; }
        }

        public static string AddRouteMenuTitle
        {
            get { return Resource.AddRouteMenuTitle; }
        }

        public static string ConfigurationMenuTitle
        {
            get { return Resource.ConfigurationMenuTitle; }
        }

        public static string BusMenuTitle
        {
            get { return Resource.BusMenuTitle; }
        }

        public static string ChangePasswordSave
        {
            get { return Resource.ChangePasswordSave; }
        }

        public static string ConfirmPasswordPlaceholder
        {
            get { return Resource.ConfirmPasswordPlaceholder; }
        }

        public static string ConfirmPassword
        {
            get { return Resource.ConfirmPassword; }
        }

        public static string NewPasswordPlaceholder
        {
            get { return Resource.NewPasswordPlaceholder; }
        }

        public static string NewPassword
        {
            get { return Resource.NewPassword; }
        }

        public static string RemoveAccountButton
        {
            get { return Resource.RemoveAccountButton; }
        }

        public static string SuccessfulChangePassword
        {
            get { return Resource.SuccessfulChangePassword; }
        }

        public static string ChangePassTitle
        {
            get { return Resource.ChangePassTitle; }
        }

        public static string ConfirmDeleteAccount
        {
            get { return Resource.ConfirmDeleteAccount; }
        }

        public static string DeleteAccountConfirmation
        {
            get { return Resource.DeleteAccountConfirmation; }
        }

        public static string RouteOriginLabel
        {
            get { return Resource.RouteOriginLabel; }
        }

        public static string RouteDestinyLabel
        {
            get { return Resource.RouteDestinyLabel; }
        }

        public static string RouteDateLabel
        {
            get { return Resource.RouteDateLabel; }
        }

        public static string RouteNumSeatsLabel
        {
            get { return Resource.RouteNumSeatsLabel; }
        }

        public static string ChatMessagesPlaceHolder
        {
            get { return Resource.ChatMessagesPlaceHolder; }
        }

        public static string ForgotPasswordLabel
        {
            get { return Resource.ForgotPasswordLabel; }
        }

        public static string ForgotPasswordMessage
        {
            get { return Resource.ForgotPasswordMessage; }
        }

        public static string ForgotPasswordPlaceHolder
        {
            get { return Resource.ForgotPasswordPlaceHolder; }
        }

        public static string ForgotPasswordButtonSend
        {
            get { return Resource.ForgotPasswordButtonSend; }
        }

        public static string ForgotPasswodButtonBack
        {
            get { return Resource.ForgotPasswodButtonBack; }
        }

        public static string ForgotPasswodTitle
        {
            get { return Resource.ForgotPasswodTitle; }
        }

        public static string DetailRouteOriginLabel
        {
            get { return Resource.DetailRouteOriginLabel; }
        }

        public static string DetailRouteDestinyLabel
        {
            get { return Resource.DetailRouteDestinyLabel; }
        }

        public static string DetailRouteDateLabel
        {
            get { return Resource.DetailRouteDateLabel; }
        }

        public static string DetailRouteSeatsLabel
        {
            get { return Resource.DetailRouteSeatsLabel; }
        }

        public static string DetailRouteEmailLabel
        {
            get { return Resource.DetailRouteEmailLabel; }
        }

        public static string DetailRouteRemoveButton
        {
            get { return Resource.DetailRouteRemoveButton; }
        }

        public static string DetailRouteEditLabel
        {
            get { return Resource.DetailRouteEditLabel; }
        }

        public static string DetailRouteChatListButton
        {
            get { return Resource.DetailRouteChatListButton; }
        }

        public static string DetailRouteChatButton
        {
            get { return Resource.DetailRouteChatButton; }
        }

        public static string AllMyConversationTitle
        {
            get { return Resource.AllMyConversationTitle; }
        }

        public static string DetailRoutesTitle
        {
            get { return Resource.DetailRoutesTitle; }
        }

        public static string RegisterEMailUcaError
        {
            get { return Resource.RegisterEMailUcaError; }
        }

        public static string UpdateProfileTitle
        {
            get { return Resource.UpdateProfileTitle; }
        }

        public static string UpdateProfileMessage
        {
            get { return Resource.UpdateProfileMessage; }
        }

        public static string EditRouteTitle
        {
            get { return Resource.EditRouteTitle; }
        }

        public static string DetailRouteDescriptionLabel
        {
            get { return Resource.DetailRouteDescriptionLabel; }
        }

        public static string HomeNavBar
        {
            get { return Resource.HomeNavBar; }
        }

        public static string AddNavBar
        {
            get { return Resource.AddNavBar; }
        }

        public static string OriginDestinyCityValidation
        {
            get { return Resource.OriginDestinyCityValidation; }
        }

    }

}
