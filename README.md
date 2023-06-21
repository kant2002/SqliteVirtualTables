# Sqlite Virtual Tables

This repository provide sample how to create virtual tables implementation for Sqlite in C#.
This is direct port of [series.c](https://sqlite.org/src/file/ext/misc/series.c) from Sqlite repo.

- SeriesModule.cs is actual implementation of the virtual table
- VirtualTableSupport.cs contains types and functions from Sqlite which is needed for make this magic works. These functions does not provided by SQLitePCLRaw.

# Additional links

- [The Virtual Table Mechanism Of SQLite](https://www.sqlite.org/vtab.html)
