FROM node:21-alpine

WORKDIR /app
COPY ./reactjs/package*.json ./

RUN npm install
COPY ./reactjs .

RUN npm run build

EXPOSE 3000

CMD ["npm", "start"]
