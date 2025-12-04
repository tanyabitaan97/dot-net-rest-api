using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using dotnet.Models;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/students")]
public class StudentsController : ControllerBase
{
    private readonly MySqlConnection _conn;

    public StudentsController(MySqlConnection conn)
    {
        _conn = conn;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        await _conn.OpenAsync();

        var cmd = new MySqlCommand("SELECT Id, Name, Age FROM students", _conn);
        var reader = await cmd.ExecuteReaderAsync();

        var list = new List<Student>();

        while (await reader.ReadAsync())
        {
            list.Add(new Student
            {
                Id = reader.GetInt32("Id"),
                Name = reader.GetString("Name"),
                Age = reader.GetInt32("Age")
            });
        }

        return Ok(list);
    }


    [HttpGet("/pagination")]
    [Authorize]
public async Task<IActionResult> GetAllWithPagination(
    int page = 1,
    int pageSize = 10,
    string sortBy = "Id",
    string sortOrder = "ASC",
    string? search = "") {

    await _conn.OpenAsync();


    var allowedSortColumns = new[] { "Id", "Name", "Age" };
    if (!allowedSortColumns.Contains(sortBy))
        sortBy = "Id";

    sortOrder = sortOrder.ToUpper() == "DESC" ? "DESC" : "ASC";

    string sql = @"
        SELECT Id, Name, Age
        FROM students
        WHERE Name LIKE @search
        ORDER BY " + sortBy + " " + sortOrder + @"
        LIMIT @offset, @pageSize;
    ";

    var cmd = new MySqlCommand(sql, _conn);
    cmd.Parameters.AddWithValue("@search", "%" + search + "%");
    cmd.Parameters.AddWithValue("@offset", (page - 1) * pageSize);
    cmd.Parameters.AddWithValue("@pageSize", pageSize);

    var reader = await cmd.ExecuteReaderAsync();

    var list = new List<Student>();
    while (await reader.ReadAsync())
    {
        list.Add(new Student
        {
            Id = reader.GetInt32("Id"),
            Name = reader.GetString("Name"),
            Age = reader.GetInt32("Age")
        });
    }

    return Ok(list);
}


    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        await _conn.OpenAsync();

        var cmd = new MySqlCommand("SELECT Id, Name, Age FROM students WHERE Id=@id", _conn);
        cmd.Parameters.AddWithValue("@id", id);

        var reader = await cmd.ExecuteReaderAsync();

        if (!reader.HasRows) return NotFound();

        await reader.ReadAsync();

        var student = new Student
        {
            Id = reader.GetInt32("Id"),
            Name = reader.GetString("Name"),
            Age = reader.GetInt32("Age")
        };

        return Ok(student);
    }


    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(Student student)
    {
        await _conn.OpenAsync();

        var cmd = new MySqlCommand(
            "INSERT INTO students (Name, Age) VALUES (@name, @age); SELECT LAST_INSERT_ID();",
            _conn
        );

        cmd.Parameters.AddWithValue("@name", student.Name);
        cmd.Parameters.AddWithValue("@age", student.Age);

        var newId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
        student.Id = newId;

        return Ok(student);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Student student)
    {
        await _conn.OpenAsync();

        var cmd = new MySqlCommand(
            "UPDATE students SET Name=@name, Age=@age WHERE Id=@id",
            _conn
        );

        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@name", student.Name);
        cmd.Parameters.AddWithValue("@age", student.Age);

        int rows = await cmd.ExecuteNonQueryAsync();
        if (rows == 0) return NotFound();

        student.Id = id;
        return Ok(student);
    }


    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _conn.OpenAsync();

        var cmd = new MySqlCommand("DELETE FROM students WHERE Id=@id", _conn);
        cmd.Parameters.AddWithValue("@id", id);

        int rows = await cmd.ExecuteNonQueryAsync();
        if (rows == 0) return NotFound();

        return Ok("Deleted");
    }
}
