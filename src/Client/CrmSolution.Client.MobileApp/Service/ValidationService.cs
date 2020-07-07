using System.Text.RegularExpressions;

namespace CrmSolution.Client.MobileApp.Service
{
    public class ValidationService
    {
        public bool IsEnglishLetters(string word)
        {
            Regex regex = new Regex(@"^[a-zA-Z ]*$", RegexOptions.Compiled);
            return regex.IsMatch(word);
        }
    }
}
