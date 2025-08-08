# Ninject Warrior Adventure

**ASP.NET Core MVC port for Ninject Warrior** (Co-developed with [Jules](https://jules.google.com/))

## Overview

`ninject-warrior` is a C# project that adapts the original Ninject Warrior design to the ASP.NET Core MVC ecosystem. The project demonstrates core gameplay logic, repositories, and strategy patterns in a modern ASP.NET Core environment—leveraging the built-in dependency injection container rather than Ninject.

## Features

- Modular architecture with repositories and service layers for game state, quests, battles, and equipment.
- Strategy pattern for flexible battle mechanics.
- Clean separation of models, services, and controllers.
- Uses built-in ASP.NET Core dependency injection.
- Data-driven: JSON files for quests, equipment, enemies, and more.
- MVC web interface for interacting with the game's adventure system.

## Getting Started

### Prerequisites

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download) or newer

### Installation

Clone the repository:
```bash
git clone https://github.com/ash-norsidi/ninject-warrior.git
cd ninject-warrior
```

Restore and build the project:
```bash
dotnet restore
dotnet build
```

### Running the Project

From the root directory, run:
```bash
dotnet run --project NinjectWarrior
```
Then visit [https://localhost:5001](https://localhost:5001) (or the specified port in your console output).

## Project Structure

- `Controllers/` — MVC controllers for the web interface.
- `Models/` — Core data models: player, inventory, quests, equipment, etc.
- `Repositories/` — Data access and aggregation from JSON files.
- `Services/` — Game logic, battle strategies, puzzle solving, adventure state.
- `Views/` — Razor views for the MVC web UI.
- `Data/` — Game data in JSON format.

## Dependency Injection

This project uses the built-in ASP.NET Core DI container (`builder.Services.AddScoped<>`) to register repositories, services, and strategies. See [`NinjectWarrior/Program.cs`](./NinjectWarrior/Program.cs) for details.

## License

MIT License

## Author

[ash-norsidi](https://github.com/ash-norsidi)
