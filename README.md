# ApiGatewayWithYarp

## Solution Overview

This solution demonstrates how to use YARP (Yet Another Reverse Proxy) as an API Gateway in a .NET 9 environment. The API Gateway is responsible for routing, authentication, and authorization of requests to backend APIs.

### Key Features

- **YARP Reverse Proxy:**  
  The API Gateway uses YARP to forward incoming requests to backend services based on route configuration in `appsettings.json`.

- **Authentication & Authorization:**  
  The gateway enforces authentication using Bearer tokens. Authorization policies are defined to restrict access to specific routes based on user claims.

- **Custom Authorization Policies:**  
  Example policies such as `first-api-access` require users to have specific claims to access certain API routes. These policies are configured in `Program.cs` and referenced in the YARP route configuration.

- **Login Endpoint:**  
  The gateway provides a `/login` endpoint that issues Bearer tokens with claims (e.g., `first-api-access`) based on query parameters. These claims determine which APIs the user can access.

- **Backend API Protection:**  
  The backend API also enforces authentication and authorization, ensuring that only requests with valid tokens and claims (as issued by the gateway) are processed.

### How It Works

1. **User Authentication:**  
   - The client sends a request to the gateway's `/login` endpoint, specifying which claims to include (e.g., `firstApi=true`).
   - The gateway returns a Bearer token containing the requested claims.

2. **Request Routing & Authorization:**  
   - The client sends API requests to the gateway (e.g., `/api/hello`), including the Bearer token in the `Authorization` header.
   - YARP forwards the request to the backend API only if the token satisfies the required authorization policy (e.g., `first-api-access`).

3. **Backend API Validation:**  
   - The backend API validates the token and claims, ensuring that only authorized users can access protected endpoints.

### Configuration Highlights

- **YARP Route Example (`appsettings.json`):**