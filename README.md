# MongoDb.Include

An Expremental project that brings **Include** and **ThenInclude** with with **Filtering** to **MongoDb** similar to **EF Core**.

*Works in progress: not yet ready for production.*

### Demo:

```csharp
public class Category
{
	public ObjectId Id { get; set; }
	public string Name { get; set; }
	
	[InverseProperty("CategoryId")]
	public ICollection Posts { get; set; }
}

public class Post
{
	public ObjectId Id { get; set; }
	public string Name { get; set; }
	public ObjectId CategoryId { get; set; }
	
	[ForeignKey("CategoryId")]
	public Category Category { get; set; }
}


var posts = postCollection
				.AsAggregateQueryable()
				.Include(p => p.Category)
				.ToList();

var categories = categoryCollection
				.AsAggregateQueryable()
				.Include(p => p.Posts)
				.ToList();
```


