using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeManager.Models;
using System.Text.RegularExpressions;

namespace WorkTimeManager.Models
{
    static class Validator
    {

        //user
        private const string LOGIN_PATTERN = @"^[a-z]{1,20}$";
        private const string POSITION_PATTERN = @"^(Programista)$|^(Starszy programista)$|^(Szef)$|^(Admin)$";
        private const string FIRST_NAME_PATTERN = @"^[A-ZŻŹĆĄŚĘŁÓŃ]{1}[a-zżźćńółęąś]{0,19}$";
        private const string LAST_NAME_PATTERN = @"^[A-ZŻŹĆĄŚĘŁÓŃ]{1}[a-zżźćńółęąś]{0,29}$";

        //customer
        private const string CUSTOMER_NAME_PATTERN = @"^[A-Z0-9][A-z0-9-, ]{0,39}$";

        //project
        private const string STATUS_PATTERN = @"^(Zakończony)$|^(W trakcie)$|^(Zaakceptowany)$";
        private const string PROJECT_NAME_PATTERN = @"^[A-Z0-9][A-z0-9-, ]{0,49}$";
        private const string PRJECT_DESCRIPTION = @"^[A-zżźćńółęąśŻŹĆĄŚĘŁÓŃ0-9 ;,.\-\/\(\)?!]{0,255}$";
        private const string HOURS_PATTERN = @"^([0-9]{1,2})$"; //0-99
        private const string SALARY_PATTERN = @"^\d*[.,]?\d*$"; //0-99
        private const string BUDGET_AND_TIME_BUDGET_PATTERN = @"^[0-9]{1,7}$";

        //common regex
        private const string NIP_PATTERN = @"^[0-9]{10}$";
        private const string ID_PATTERN = @"^[0-9]{10}$";
        private const string MAIL_PATTERN = @"^[A-z0-9.-_]+[@][A-z]+[.][A-z]{2,9}$";
        private const string PHONE_PATTERN = @"^([+]{1}[0-9]{2})[0-9]{9}$";
        private const string ADDRESS_PATTERN = @"^[A-zżźćńółęąśŻŹĆĄŚĘŁÓŃ0-9 -.,\/]{1,255}$";

        static public bool IsUserValid(User user)
        {
            if (!IsValid("login", user.Login)) return false;
            if (!IsValid("firstname", user.FirstName)) return false;
            if (!IsValid("lastname", user.LastName)) return false;
            if (!IsValid("email", user.Email)) return false;
            if (!IsValid("phone", user.PhoneNumber)) return false;
            if (!IsValid("address", user.Address)) return false;
            if (!IsValid("position", user.Position)) return false;
            if (!IsValid("salary", user.Salary.ToString())) return false;
            if (!IsValid("hours", user.Hours.ToString())) return false;
            return true;
        }

        static public bool IsCustomerValid(Customer customer)
        {
            if (!IsValid("nip", customer.NIP)) return false;
            else if (!IsValid("customername", customer.Name)) return false;
            else if (!IsValid("email", customer.Email)) return false;
            else if (!IsValid("phone", customer.PhoneNumber)) return false;
            else if (!IsValid("address", customer.Address)) return false;
            return true;
        }

        static public bool IsProjectValid(Project project)
        {
            if (!IsValid("projectname", project.Name)) return false;
            else if (!IsValid("status", project.Status)) return false;
            else if (!IsValid("budget", project.Budget.ToString())) return false;
            else if (!IsValid("budget", project.TimeBudget.ToString())) return false;
            else if (!IsValid("description", project.Description)) return false;
            else if (!IsValid("nip", project.CustomerNIP)) return false;
            return true;
        }

        static public bool IsValid(string option, string textToValidate)
        {
            string pattern = "*";
            if (option.ToLower() == "login") pattern = LOGIN_PATTERN;
            else if (option.ToLower() == "id") pattern = ID_PATTERN;
            else if (option.ToLower() == "firstname") pattern = FIRST_NAME_PATTERN;
            else if (option.ToLower() == "lastname") pattern = LAST_NAME_PATTERN;
            else if (option.ToLower() == "customername") pattern = CUSTOMER_NAME_PATTERN;
            else if (option.ToLower() == "projectname") pattern = PROJECT_NAME_PATTERN;
            else if (option.ToLower() == "description") pattern = PRJECT_DESCRIPTION;
            else if (option.ToLower() == "status") pattern = STATUS_PATTERN;
            else if (option.ToLower() == "budget") pattern = BUDGET_AND_TIME_BUDGET_PATTERN;
            else if (option.ToLower() == "nip") pattern = NIP_PATTERN;
            else if (option.ToLower() == "email") pattern = MAIL_PATTERN;
            else if (option.ToLower() == "phone") pattern = PHONE_PATTERN;
            else if (option.ToLower() == "address") pattern = ADDRESS_PATTERN;
            else if (option.ToLower() == "position") pattern = POSITION_PATTERN;
            else if (option.ToLower() == "salary") pattern = SALARY_PATTERN;
            else if (option.ToLower() == "hours") pattern = HOURS_PATTERN;

            Match validationRresult = Regex.Match(textToValidate, pattern);
            if (validationRresult.Success)
                return true;
            return false;
        }
    }
}