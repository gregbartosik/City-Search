namespace CitySearchAPI.Data
{
    public class LoginResult
    {
        
        /// TRUE if the login attempt is successful, FALSE otherwise.
        public bool Success { get; set; }

        
        /// Login attempt result message
        public string Message { get; set; } = null!;
        
        /// The JWT token if the login attempt is successful, or NULL if not
        public string? Token { get; set; }
    }
}
