using GeneTree.BLL.DTO;
using GeneTree.BLL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneTree.Presentation.Infrastructure
{
    internal class GeneApp
    {
        private readonly IGeneService _geneService;

        public GeneApp(IGeneService geneService)
        {
            _geneService = geneService;
        }

        public async Task RunAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Genealogy Tree Application");
                Console.WriteLine("1. Add Person");
                Console.WriteLine("2. Add Parent-Child Relationship");
                Console.WriteLine("3. Add Spouse Relationship");
                Console.WriteLine("4. Display Tree");
                Console.WriteLine("5. Show Immediate Relatives");
                Console.WriteLine("6. Calculate Ancestor Age");
                Console.WriteLine("7. Clear Tree");
                Console.WriteLine("8. Exit");
                Console.Write("Choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out var choice))
                {
                    Console.WriteLine("Invalid input. Press any key to try again.");
                    Console.ReadKey();
                    continue;
                }

                try
                {
                    switch (choice)
                    {
                        case 1:
                            await AddPersonAsync();
                            break;
                        case 2:
                            await AddParentChildRelationshipAsync();
                            break;
                        case 3:
                            await AddSpouseRelationshipAsync();
                            break;
                        case 4:
                            await DisplayTreeAsync();
                            break;
                        case 5:
                            await ShowImmediateRelativesAsync();
                            break;
                        case 6:
                            await CalculateAncestorAgeAsync();
                            break;
                        case 7:
                            await ClearTreeAsync();
                            break;
                        case 8:
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Press any key to try again.");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        private async Task AddPersonAsync()
        {
            Console.Write("Enter full name: ");
            var fullName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(fullName))
            {
                Console.WriteLine("Full name cannot be empty. Press any key to return.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter date of birth (yyyy-MM-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out var dob))
            {
                Console.WriteLine("Invalid date format. Press any key to return.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter gender: ");
            var gender = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(gender))
            {
                Console.WriteLine("Gender cannot be empty. Press any key to return.");
                Console.ReadKey();
                return;
            }

            var person = new PersonDto
            {
                FullName = fullName,
                DateOfBirth = dob,
                Gender = gender
            };
            var personId = await _geneService.AddPersonAsync(person);

            Console.WriteLine($"Person added successfully with ID: {personId}. Press any key to continue...");
            Console.ReadKey();
        }


        private async Task AddParentChildRelationshipAsync()
        {
            Console.Write("Enter parent ID: ");
            if (!int.TryParse(Console.ReadLine(), out var parentId))
            {
                Console.WriteLine("Invalid parent ID. Press any key to return.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter child ID: ");
            if (!int.TryParse(Console.ReadLine(), out var childId))
            {
                Console.WriteLine("Invalid child ID. Press any key to return.");
                Console.ReadKey();
                return;
            }

            try
            {
                await _geneService.AddParentChildRelationshipAsync(parentId, childId);
                Console.WriteLine("Parent-child relationship added successfully. Press any key to continue...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}. Press any key to return.");
            }

            Console.ReadKey();
        }

        private async Task AddSpouseRelationshipAsync()
        {
            Console.Write("Enter Spouse1 ID: ");
            if (!int.TryParse(Console.ReadLine(), out var spouse1Id))
            {
                Console.WriteLine("Invalid Spouse1 ID. Press any key to return.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter Spouse2 ID: ");
            if (!int.TryParse(Console.ReadLine(), out var spouse2Id))
            {
                Console.WriteLine("Invalid child ID. Press any key to return.");
                Console.ReadKey();
                return;
            }

            try
            {
                await _geneService.AddSpouseRelationshipAsync(spouse1Id,spouse2Id);
                Console.WriteLine("Parent-child relationship added successfully. Press any key to continue...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}. Press any key to return.");
            }

            Console.ReadKey();
        }


        private async Task DisplayTreeAsync()
        {
            var tree = await _geneService.GenerateTreeAsync();

            if (string.IsNullOrWhiteSpace(tree))
            {
                Console.WriteLine("No genealogy data available.");
            }
            else
            {
                Console.WriteLine("Genealogy Tree ");
                Console.WriteLine(tree);
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }


        private async Task ShowImmediateRelativesAsync()
        {
            Console.Write("Enter person ID: ");
            if (!int.TryParse(Console.ReadLine(), out var personId))
            {
                Console.WriteLine("Invalid person ID. Press any key to return.");
                Console.ReadKey();
                return;
            }

            var relatives = await _geneService.GetImmediateRelativesAsync(personId);

            if (!relatives.Any())
            {
                Console.WriteLine("No relatives found for this person.");
            }
            else
            {
                Console.WriteLine("Immediate Relatives (IDs included):");
                foreach (var relative in relatives)
                {
                    Console.WriteLine($"[{relative.Id}] {relative.FullName} (DOB: {relative.DateOfBirth:yyyy-MM-dd}, Gender: {relative.Gender})");
                }
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }



        private async Task CalculateAncestorAgeAsync()
        {
            Console.Write("Enter ancestor ID: ");
            if (!int.TryParse(Console.ReadLine(), out var ancestorId))
            {
                Console.WriteLine("Invalid ancestor ID. Press any key to return.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter descendant ID: ");
            if (!int.TryParse(Console.ReadLine(), out var descendantId))
            {
                Console.WriteLine("Invalid descendant ID. Press any key to return.");
                Console.ReadKey();
                return;
            }

            var ageDifference = await _geneService.CalculateAncestorAge(ancestorId, descendantId);
            Console.WriteLine($"The ancestor was {ageDifference} years old when the descendant was born.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private async Task ClearTreeAsync()
        {
            Console.WriteLine("Are you sure you want to clear the entire genealogy tree? (y/n)");
            var input = Console.ReadLine()?.ToLower();

            if (input == "y")
            {
                await _geneService.ClearTreeAsync();
                Console.WriteLine("Genealogy tree cleared. Press any key to continue...");
            }
            else
            {
                Console.WriteLine("Operation cancelled. Press any key to return.");
            }

            Console.ReadKey();
        }

    }
}
