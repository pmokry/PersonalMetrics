using System.Diagnostics.CodeAnalysis;

namespace BookShelf.Repository;

public class Author
{
    public Author(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public Author(string firstName, string? middleName, string lastName)
    {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
    }

    public Author(string fullName)
    {
        if (fullName.Contains(','))
        {
            string[] names = fullName.Split(',');
            if (names.Length == 2)
            {
                string[] firstAndMiddleN = names[1].Trim().Split(' ');
                if (firstAndMiddleN.Length == 2)
                {
                    FirstName = firstAndMiddleN[0].Trim();
                    MiddleName = firstAndMiddleN[1].Trim();
                }
                else if (firstAndMiddleN.Length == 1)
                {
                    FirstName = names[1].Trim();
                }
                else
                {
                    throw new UnsupportedAuthorFullNameFormat("FullName must be in the format 'Last, First' or 'Last, First Middle'");
                }

                LastName = names[0].Trim();
            }
            else
            {
                throw new UnsupportedAuthorFullNameFormat("FullName must be in the format 'Last, First' or 'Last, First Middle'");
            }
            return;
        }

        string[] combinedNames = fullName.Split(' ');
        if (combinedNames.Length == 2)
        {
            FirstName = combinedNames[0];
            LastName = combinedNames[1];
        }
        else if (combinedNames.Length == 3)
        {
            FirstName = combinedNames[0];
            MiddleName = combinedNames[1];
            LastName = combinedNames[2];
        }
        else
        {
            throw new UnsupportedAuthorFullNameFormat("FullName must be in the format 'First Last' or 'First Middle Last'");
        }
    }


    [NotNull]
    public string FirstName { get; init; }
    public string? MiddleName { get; init; }
    [NotNull]
    public string LastName { get; init; }
    public string FullName
    {
        get { return $"{LastName}, {FirstName}" + (string.IsNullOrEmpty(MiddleName) ? string.Empty : $" {MiddleName}"); }
    }

    [Serializable]
    public class UnsupportedAuthorFullNameFormat : Exception
    {
        public UnsupportedAuthorFullNameFormat(string? message) : base(message)
        {
        }
    }
}