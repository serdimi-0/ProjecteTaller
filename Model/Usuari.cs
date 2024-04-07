namespace Model
{
    public enum TipusUsuari
    {
        ADMIN,
        MECANIC,
        RECEPCIO
    }

    public class Usuari
    {
        private string login;
        private string password;
        private TipusUsuari tipus;

        public Usuari(string login, string password)
        {
            this.login = login;
            this.password = password;
        }

        public string Login { get => login; set => login = value; }
        public string Password { get => password; set => password = value; }
        public TipusUsuari Tipus { get => tipus; set => tipus = value; }
    }
}
