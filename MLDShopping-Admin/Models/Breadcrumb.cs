namespace MLDShopping_Admin.Models
{
    public class Breadcrumb
    {
        public string Text { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Area { get; set; }
        public bool Active { get; set; }
    }
}