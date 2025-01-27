# Overview  
An eCommerce application built with C# .NET, using **AWS S3** for static object storage and **Cognito** for User authentication.  

## Architecture  
<div align="center">

![Diagram](https://github.com/user-attachments/assets/77d3e459-338e-4ecc-ae57-20bb92d84342)

</div>

### Core Components  
1. **clientBrowser**: User-facing interface for interactions using Swagger.  
2. **awsCognito**: Handles authentication/authorization.  
3. **awsS3**: Stores static assets like images.  
4. **developmentEnvironment**:  
   - **gitRepo**: Version control.  
   - **dockerContainer**: Containerized environment (⚠️ working with issues).  
   - **sqlServer**: Local development database.  
   - **ciCdPipeline**: Planned for future (🔴 not implemented).  
5. **dotnetApplication**:  
   - **backendApi**: Core API endpoints.  

## Status Indicators  
- ✅ **Done**  
- ⚠️ **Working with issues**
- 🔴 **Not implemented**

### Git Branching Strategy  
- **main**: Stable production branch.  
- **release/v{major}.{minor}.{patch}**: Prep for releases (e.g., `release/v1.2.0`).  
- **dev**: Integration branch for ongoing work.  
- **feature/***: Feature-specific branches (e.g., `feature/addCart`).  
- **hotfix/***: Emergency fixes (e.g., `hotfix/securityPatch`).  

---  

## Prerequisites  
- [.NET SDK](https://dotnet.microsoft.com/download)  
- [Docker](https://www.docker.com/get-started)  
- [AWS CLI](https://aws.amazon.com/cli/)  
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)  
