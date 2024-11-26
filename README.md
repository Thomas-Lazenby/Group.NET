# Project Overview

This library provides a dynamic data structure for creating and managing hierarchical relationships involving fields and groups, with a parent-child relationship. 
Designed for maximum flexibility, it supports nested grouping of data as well as key-value storage, making it ideal for scenarios that require a combination of flat and nested data representations.

The scope of this library is to offer an effective solution for managing hierarchical groupings with direct key-value relationships. 
It is suitable for applications involving configuration management, organisational structures, and scenarios that require flexible data organization and easy navigation through parent-child relationships.

# Features

### Hierachel Group Managment
Allows you to create a hierarchical relationship of groups and fields, enabling dynamic parent-child relationships.

```csharp var rootGroup = new Group<string, object>();

// Adding subgroups
var subgroup1 = rootGroup.CreateSubGroup("Subgroup1");
var subgroup2 = rootGroup.CreateSubGroup("Subgroup2");

// Setting parent-child relationships
Console.WriteLine(subgroup1.Parent == rootGroup); // Output: True
Console.WriteLine(rootGroup.IsRootGroup); // Output: True
Console.Writeline(subGroup1.GetRootGroup() == rootGroup) // Output: True
```

### Key-Value Field Storage
Store data as key-value pairs in a flexible and dynamic way.

```csharp
var group = new Group<string, object>();

// Adding fields
group.AddField("Username", "JohnDoe");
group.AddField("Age", 30);

// Retrieving fields
var username = group.GetField<string>("Username");
Console.WriteLine(username); // Output: JohnDoe

// Updating fields
group.UpdateField("Age", 31);
var age = group.GetField<int>("Age");
Console.WriteLine(age); // Output: 31
```

### Type-Safe Operations 
```csharp
var group = new Group<string, object>();

group.AddField("IsActive", true);

// Retrieve field with type-safety
if (group.TryGetField<bool>("IsActive", out bool isActive))
{
    Console.WriteLine(isActive); // Output: True
}
else
{
    Console.WriteLine("Field not found or type mismatch.");
}
```

# Comparison to Alternatives

### Tree

How this Library Differs:
- Node Clarity with Groups and Fields: The Group structure differentiates between fields (key-value attributes) and subgroups (nested groups). This clear separation helps manage complex hierarchies and metadata more effectively, avoiding the ambiguity of treating everything as a generic "node".
- Bidirectional Navigation: Unlike traditional trees that only allow downward traversal, the Group structure also supports navigating upwards through the Parent reference.

### Dictionary

How this Library Differs:
- Flat vs. Hierarchical: Standard dictionaries are ideal for flat key-value storage but do not inherently support nested relationships. The Group structure allows for hierarchical organisation by embedding subgroups, making it suitable for both simple lookups and complex nested data.
- Context-Aware Key Usage: In a dictionary, all keys exist at the same level, which limits flexibility. In contrast, Group allows keys to have different contexts either as fields or as subgroups providing expressiveness in modeling relationships.

### Composite Pattern

- Data-Rich Nodes: Unlike typical Composite Pattern implementations, where leaves and composites are stored uniformly, Group nodes distinguish between fields and subgroups, storing them separately. This improves performance for lookups and keeps data better organized.
- Type Safety and Flexibility: The Group class employs generics (TKey and TValue) to provide type-safe and flexible usage, unlike the ungeneric, fixed constraints often found in Composite implementations. This allows better adaptability for varied use cases.

# How to Use

N/A

# Contribution

N/A

# License

Group.NET is license under the [MIT License](https://github.com/Thomas-Lazenby/Group.NET/blob/readme-md/LICENSE.txt).
