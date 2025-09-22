using Restaurant_Chain_Management.Models.Enums;

namespace Restaurant_Chain_Management.Models.Data
{
    public static class DbInitializer
    {
    //    public static async Task SeedAsync(AppDbContext db)
    //    {
    //        if (db.Cities.Any() || db.Branches.Any() || db.Employees.Any())
    //            return; // لو فيه بيانات مسبقة نتخطى

    //        // 1) Cities
    //        var cities = new[]
    //        {
    //    new City { Name = "Cairo" },
    //    new City { Name = "Alexandria" },
    //    new City { Name = "Giza" }
    //};
    //        db.Cities.AddRange(cities);
    //        await db.SaveChangesAsync();

    //        // 2) Branches
    //        var branches = new[]
    //        {
    //    new Branch { Name = "Cairo - Downtown", CityId = cities[0].Id },
    //    new Branch { Name = "Cairo - Maadi", CityId = cities[0].Id },
    //    new Branch { Name = "Alexandria - Raml", CityId = cities[1].Id },
    //    new Branch { Name = "Giza - Dokki", CityId = cities[2].Id },
    //};
    //        db.Branches.AddRange(branches);
    //        await db.SaveChangesAsync();

    //        // 3) Employees
    //        var employees = new[]
    //        {
    //    new Employee { Name = "Fady Emil", Title = "General Manager", Role = EmployeeRole.GeneralManager },

    //    new Employee { Name = "Amr Hassan", Title = "Branch Manager", Role = EmployeeRole.BranchManager, BranchId = branches[0].Id },
    //    new Employee { Name = "Sara Mohamed", Title = "Branch Manager", Role = EmployeeRole.BranchManager, BranchId = branches[2].Id },

    //    new Employee { Name = "Mona Ali", Title = "Chef", Role = EmployeeRole.Staff, BranchId = branches[0].Id },
    //    new Employee { Name = "Khaled Omar", Title = "Waiter", Role = EmployeeRole.Staff, BranchId = branches[1].Id },
    //    new Employee { Name = "Nour Samir", Title = "Cashier", Role = EmployeeRole.Staff, BranchId = branches[3].Id }
    //};
    //        db.Employees.AddRange(employees);
    //        await db.SaveChangesAsync();

    //        // 4) Assign Managers
    //        branches[0].ManagerId = employees.First(e => e.Name == "Amr Hassan").Id;
    //        branches[2].ManagerId = employees.First(e => e.Name == "Sara Mohamed").Id;

    //        db.Branches.UpdateRange(branches);
    //        await db.SaveChangesAsync();
    //    }

    }
}
