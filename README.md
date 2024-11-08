# Task Management API

A RESTful API built with ASP.NET Core for managing tasks/todos. This API provides basic CRUD operations for tasks with input validation and error handling.

## Features

- ✨ Create, Read, Update, and Delete tasks
- 🔍 Get individual tasks by ID
- ✅ Input validation for task creation
- 🔄 URL rewriting (tasks → todos)
- 📝 Request logging middleware
- 🎯 Dependency injection for task service

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/todos` | Get all tasks |
| GET | `/api/todos/{id}` | Get a specific task by ID |
| POST | `/api/todos` | Create a new task |
| DELETE | `/api/todos/{id}` | Delete a task |

### Task Model 