namespace WSW
{
    public class Constants
    {
        public enum PathInfoEnum
        {
            None = 0,
            File = 1,
            Directory = 2
        }

        // HTML Tags
        public const string HtmlBrTag = "</br>";

        // Event Viewer
        public const string UserNa = "N/A";
        public const string SourceDoesNotExist = "Source '{0}'does not exist";
        public const int TimeIntervalForLogsToCheck = -1;

        // Service Controller
        public const string RegistryPathTemplate = @"SYSTEM\CurrentControlSet\Services\{0}";
        public const string ImagePath = "ImagePath";

        // Email
        public const string Restart = "Restart";
        public const string MsgServiceRestartSuccess = "Success";
        public const string MsgServiceRestartFail = "Failed";
        public const string SubjectTemplate = "{0} [STOP]";
        public const string BodyTemplate = @"<table>
	                                  <tr>
                                          <td>{0}</td>
		                                  <td bgcolor='{1}'><b>{2}</b></td>
	                                  </tr>
                                  </table>
                                  </br>
                                  {3}";
        public const string TitleFormat = "<h2>{0}</h2>";
        public const string FileHyperlinkFormat = @"<a href='file:\\\{0}'>{1}</a>";
        public const string Green = "B8F676";
        public const string Red = "FB0000";
        public const string White = "White";
        public const string Grey = "F9F7EF";
        public const string HeaderColor = "D4C8EC";

        // Custom Configuration
        public const string EmailSection = "email";
        public const string SecureSection = "secure";
        public const string ServiceSection = "serviceSection";

        // Other constants
        public const string WswService = "WSWService";
        public const string ConfigExtension = ".config";
    }
}
