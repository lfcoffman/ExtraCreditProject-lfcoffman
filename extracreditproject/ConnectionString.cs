namespace extracreditproject
{
    public class ConnectionString
    {
        public string cs {get; set;}
        public ConnectionString(){
            string server = "w3epjhex7h2ccjxx.cbetxkdyhwsb.us-east-1.rds.amazonaws.com";
            string database = "gueaswfmik518bcd";
            string username = "ogfecy02ed2i8g0y";
            string password = "w7y3shhcxpquy35y";
            string port = "3306";
            
            cs=$"server={server};user={username};database={database};port={port};password={password}";
        }
    }
}