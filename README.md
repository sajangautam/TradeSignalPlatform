# Trade Signal Platform

Root repository for the Trade Signal Manager API and related components.

## Project Structure
TradeSignalPlatform/
└── TradeSignalManager.Api/ # .NET Web API
├── Controllers/ # API endpoints
├── Program.cs # Startup config
└── Properties/ # Launch settings


## Running the API
1. Navigate to the API project:
   cd TradeSignalManager.Api

2. Start the application:
   dotnet run

3. Access the API:
   Swagger UI: http://localhost:5000/swagger

## Running backend with docker
1. Navigate to the root folder of the project:
   cd TradeSignalPlatform

2. Build the container:
   docker build -t tradesignal-api .

3. Remove the existing container:
   docker rm -f tradesignal-api-container

4. Run the container:
   docker run -d -p 5000:8080 -e ASPNETCORE_ENVIRONMENT=Development --name tradesignal-api-container tradesignal-api

## Running whole application with docker
1. Navigate to the root folder of the project:
   cd TradeSignalPlatform

2. Build the container:
   docker compose up --build

3. Remove the existing container:
   docker compose down

## Running whole application with kubernetes
1. Navigate to the root folder of the project:
   cd TradeSignalPlatform

2. Build the docker images:
   docker build -t tradesignal-backend:latest .
   docker build -t tradesignal-client:latest .

3. Apply your Kubernetes manifests:
   kubectl apply -f k8s/backend-deployment.yaml
   kubectl apply -f k8s/frontend-deployment.yaml

4. Apply your Kubernetes manifests:
   kubectl delete pods -l app=tradesignal-backend
   kubectl delete pods -l app=tradesignal-frontend

5. Verify pods and services are running:
   kubectl get pods
   kubectl get services

