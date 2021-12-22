Oracle Entity Framework Core 6.21.4 README
==========================================
Release Notes: Oracle Entity Framework Core 6 Provider

November 2021 

This README supplements the main ODP.NET 21c documentation.
https://docs.oracle.com/en/database/oracle/oracle-database/21/odpnt/


New Features
============
1) Entity Framework Core 6
Oracle Entity Framework (EF) Core now supports Entity Framework Core 6 with 
Oracle databases. Entity Framework Core is a cross-platform Microsoft 
object-relational mapper that enables .NET developers to work with relational 
databases using .NET objects. 


Bugs Fixed since Oracle.EntityFrameworkCore NuGet Package 5.21.4
================================================================
None


Tips, Limitations, and Known Issues
===================================
Code First
----------
* The HasIndex() Fluent API cannot be invoked on an entity property that will result in a primary key in the Oracle database. 
Oracle Database does not support index creation for primary keys since an index is implicitly created for all primary keys.

* Oracle Database 11.2 does not support default expression to reference any PL/SQL functions nor any pseudocolumns such as 
'<sequence>.NEXTVAL'. As such, HasDefaultValue() and HasDefaultValueSql() Fluent APIs cannot be used in conjunction with 
'sequence.NEXTVAL' as the default value, for example, with the Oracle Database 11.2. However, the application can use the 
UseOracleIdentityColumn() extension method to have the column be populated with server generated values even for Oracle 
Database 11.2. Please read about UseOracleIdentityColumn() for more details.

* The HasFilter() Fluent API is not supported. For example, 
modelBuilder.Entity<Blog>().HasIndex(b => b.Url.HasFilter("Url is not null");

* Data seeding using the UseIdentityColumn is not supported.

* The UseCollation() Fluent API is not supported.

* The new DateOnly and TimeOnly types from .NET 6 are not supported.

Computed Columns
----------------
* Literal values used for computed columns must be encapsulated by two single-quotes. In the example below, the literal string 
is the comma. It needs to be surrounded by two single-quotes as shown below.

     // C# - computed columns code sample
    modelBuilder.Entity<Blog>()
    .Property(b => b.BlogOwner)
    .HasComputedColumnSql("\"LastName\" || '','' || \"FirstName\"");

Database Scalar Function Mapping
--------------------------------
* Database scalar function mapping does not provide a native way to use functions residing within PL/SQL packages. To work around 
this limitation, map the package and function to an Oracle synonym, then map the synonym to the EF Core function.

LINQ
----
* Oracle Database 12.1 has the following limitation: if the select list contains columns with identical names and you specify the 
row limiting clause, then an ORA-00918 error occurs. This error occurs whether the identically named columns are in the same table 
or in different tables.

Let us suppose that database contains following two table definitions:
SQL> desc X;
 Name    Null?    Type
 ------- -------- ----------------------------
 COL1    NOT NULL NUMBER(10)
 COL2             NVARCHAR2(2000)

SQL> desc Y;
 Name    Null?    Type
 ------- -------- ----------------------------
 COL0    NOT NULL NUMBER(10)
 COL1             NUMBER(10)
 COL3             NVARCHAR2(2000)

Executing the following LINQ, for example, would generate a select query which would contain "COL1" column from both the tables. 
Hence, it would result in error ORA-00918:
dbContext.Y.Include(a => a.X).Skip(2).Take(3).ToList();
This error does not occur when using Oracle Database 11.2, 12.2, and higher versions.

* Certain LINQs cannot be executed against Oracle Database 11.2.
Let us first imagine an Entity Model with the following entities:

public class Gear
{
    public string FullName { get; set; }
    public virtual ICollection<Weapon> Weapons { get; set; }
}

public class Weapon
{
    public int Id { get; set; }
    public bool IsAutomatic { get; set; }
    public string OwnerFullName { get; set; }
    public Gear Owner { get; set; }
}

The following LINQ will not work against Oracle Database 11.2:

dbContext.Gear.Include(i => i.Weapons).OrderBy(o => o.Weapons.OrderBy(w => w.Id).FirstOrDefault().IsAutomatic).ToList();

This is due to LINQ creating the following SQL query:

SELECT "i"."FullName"
FROM "Gear" "i"
ORDER BY (
    Select
     K0 "IsAutomatic" from(
    SELECT "w"."IsAutomatic" K0
    FROM "Weapon" "w"
    WHERE ("i"."FullName" = "w"."OwnerFullName")
    ORDER BY "w"."Id" NULLS FIRST
    ) "m1"
    where rownum <= 1
) NULLS FIRST, "i"."FullName" NULLS FIRST

Within the SELECT statement, there are two nested SELECTs. The generated SQL will encounter a ORA-00904 : 
"invalid identifier" error with Oracle Database 11.2 since it has a restriction where it does not recognize outer 
select table alias "i" in the inner nested select query.

* LINQ query's that are used to query or restore historical (Temporal) data are not supported.

* LINQ query's that are used to query the new DateOnly and TimeOnly types from .NET 6 are not supported.

Migrations
----------
* If more than one column is associated with any sequence/trigger, then ValueGeneratedOnAdd() Fluent API will be generated 
for each of these columns when performing a scaffolding operation. If we then use this scaffolded model to perform a migration, 
then an issue occurs. Each column associated with the ValueGeneratedOnAdd() Fluent API is made an identity column by default. 
To avoid this issue, use UseOracleSQLCompatibility("11") which will force Entity Framework Core to generate triggers/sequences 
instead.

Scaffolding
-----------
* Scaffolding a table that uses Function Based Indexes is supported. However, the index will NOT be scaffolded.
* Scaffolding a table that uses Conditional Indexes is not supported.

Sequences
---------
* A sequence cannot be restarted.
* Extension methods related to SequenceHiLo is not supported, except for columns with Char, UInt, ULong, and UByte data types.

 Copyright (c) 2021, Oracle and/or its affiliates.
