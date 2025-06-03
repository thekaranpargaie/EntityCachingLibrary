# EntityCachingLibrary

A lightweight and extensible .NET library that simplifies caching of entities using generic patterns and abstraction. Designed for easy integration and scalability in modern .NET applications.

## 🚀 Features

* Easy-to-use generic entity caching
* Interface-based architecture for flexibility and testability
* Plug-and-play integration with .NET Core or .NET Framework
* Clean separation of concerns with service and model layers

## 💠 Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/thekaranpargaie/EntityCachingLibrary.git
   ```

2. Open the solution (`EntityCachingLib.sln`) in Visual Studio.

3. Build the project and reference it in your application.

## 📆 Usage

Example of using the cache service:

```csharp
IEntityCacheService<MyEntity> cacheService = new EntityCacheService<MyEntity>();

// Store an entity
cacheService.Set("entity-key", new MyEntity());

// Retrieve it
var entity = cacheService.Get("entity-key");

// Remove it
cacheService.Remove("entity-key");
```

> Replace `MyEntity` with your own entity type.

## 📁 Project Structure

* `Interfaces/` – Abstractions for cache behavior
* `Services/` – Implementations for caching
* `Models/` – Sample or support models
* `Extensions/` – Extension methods for added functionality

## 🤝 Contributing

Contributions are welcome! Feel free to fork the repo, submit issues, or open pull requests to improve the library.

## 📄 License

This project is licensed under the **MIT License**.
See the [LICENSE](LICENSE) file for details.

---

## 📬 Contact

Created by **Karan Pargaie**.
For feedback or collaboration, feel free to reach out via GitHub issues.
