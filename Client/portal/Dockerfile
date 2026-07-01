FROM docker.io/library/node:22-alpine AS build
WORKDIR /app

COPY package.json package-lock.json ./
RUN npm ci

COPY . .
RUN cp src/environment.docker.ts src/environment.ts
RUN npm run build

FROM docker.io/library/nginx:alpine AS final
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist/portal/browser /usr/share/nginx/html

EXPOSE 80
