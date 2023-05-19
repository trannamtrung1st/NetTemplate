# NetTemplate

## Overview

- Architecture: 
   - Clean Architecture
   - CQRS
   - etc ...

## Components

1. Core: core project of the application
   - Events
   - Exceptions
   - Constants
   - Entities
   - Models
   - Handlers
   - Interfaces
     - Repositories
     - Services
     - IUnitOfWork: final UoW
   - Utils: helpers/utils and extensions
2. Business (use cases)
   - Models
   - Commands
   - Queries
   - Mapping
   - Jobs
   - Events
   - Handlers
3. Infrastructure
   - Clients
   - Implementations
   - Handlers
   - Background: jobs, job runners
   - Pub/Sub
   - Caching
   - Logging
   - AspectOriented
   - Persistence
     - EntityFramework
     - Dapper 
     - Query filters
     - Ignore query filters
   - etc ...
4. Applications
   - Apis/Services
     - Controllers
     - Attributes
     - Auth
     - Filters
     - Middlewares
     - Implementations
     - Models
     - Services
     - Constants
   - Client apps
5. Common: common services, helpers, utilities, etc => tend to be library code without business related logics
   - Crypto
   - Logging
   - Web
   - AspectOriented

## Deployment
- Dockerize applications for better isolation and management