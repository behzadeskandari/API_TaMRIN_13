namespace API.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int UserId { get; set; }

        public DateTime DateofBirth { get; set; }

        public int Telephone { get; set; }
    }
}
