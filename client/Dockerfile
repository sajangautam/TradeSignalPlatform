# Stage 1: Build the React app
FROM node:18 AS build
WORKDIR /app

# copy package files first for faster rebuilds
COPY package*.json ./
RUN npm ci

# copy the rest
COPY . .

# Allow injecting API URL at build time (this writes a .env used by CRA)
ARG REACT_APP_API_URL=http://localhost:30001
RUN echo "REACT_APP_API_URL=${REACT_APP_API_URL}" > .env

RUN npm run build

# Stage 2: Serve with nginx
FROM nginx:alpine
# remove default nginx static files (optional)
RUN rm -rf /usr/share/nginx/html/*

# copy built files from build stage
COPY --from=build /app/build /usr/share/nginx/html

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
