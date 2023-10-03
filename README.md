# Chirp

// For later use by us to make commit messages easier to write.
Co-authored-by: Mathilde <mgon@itu.dk>
Co-authored-by: Carmen <alcn@itu.dk>
Co-authored-by: Olivia <olmo@itu.dk>
Co-authored-by: Emilie <emru@itu.dk>
Co-authored-by: Alex <alebj@itu.dk>



// Husk at noter at test er navnviget og arrangeret ud fra microsoft anbefalinger
// link: https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-dotnet-test

### User manual:

Load scripts:
Navigate to Chirp/Chirp.SQLite/scripts and run the command: 
./initDB.sh

Build:
In Chirp folder run the command: dotnet build

Run program:
Navigate to Chirp/Chirp.Razor and run the command: dotnet run 

The program runs on http://localhost:5000 
(The public timeline, page 1 with the latest 32 cheeps are shown as default)
To change to page number 2, add ?page=2 in URL
To see cheeps from the user with auther id = 10, 
add /10 in the URL