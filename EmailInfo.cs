namespace EmailScanner
{
    public class EmailInfo
    {
        public string userName;
        
        public string password;

        public string serverName;

        public string port;


        /// <summary>
        /// Constructor for email info object
        /// </summary>
        /// <param name="args">string[4] containing name, password, server and port</param>
        public EmailInfo(string[] args){

            userName = args[0];

            password = args[1];

            serverName = args[2];

            port = args[3];

        }


    }
}