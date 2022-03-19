namespace UniversitySystem.Services
{
    public interface IClaimDecorator
    {
        public int Id { get; }
        
        public string Name { get; }
        
        public string Role { get; }
    }
}