namespace CQSService.common
{
    public class commonFunction
    {

        //this function Convert to Encord your Password
        public static string Base64Encode(string password)
        {
            try
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(password);
                return System.Convert.ToBase64String(plainTextBytes);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }

        //this function Convert to Decord your Password
        public static string Base64Decode(string password)
        {
            try
            {
                var base64EncodedBytes = System.Convert.FromBase64String(password);
                return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Base64Decode" + ex.Message);
            }
        }
    }
}
