# TaskManagements

## 🚀 Desafio Técnico .NET (C#) – Execução com Docker

---

## 📖 Descrição

Este projeto foi desenvolvido como parte de um processo seletivo técnico. A aplicação é uma API construída com **.NET 8**, estruturada com base em uma arquitetura em camadas (**Domain, Application, Infrastructure, API**), com boas práticas de **Clean Architecture** e foco em escalabilidade e manutenibilidade.

O ambiente completo (API e banco de dados PostgreSQL) é executado via **Docker Compose**.

---

## 🧱 Tecnologias Utilizadas

- .NET 8
- PostgreSQL 16 (imagem Alpine)
- Docker + Docker Compose
- Swagger para documentação de API
- Arquitetura em camadas com princípios SOLID

---

## 🐳 Execução com Docker Compose

### ✅ Pré-requisitos

- Docker instalado: [https://www.docker.com](https://www.docker.com)
- Docker Compose instalado (ou integrado ao Docker Desktop)

---

### ▶️ Passos para executar o projeto

1. **Clone o repositório**:

```bash
git clone https://github.com/jeanalgoritmo/TaskManagements.git
cd TaskManagements


2.**Execute o ambiente com Docker Compose**:

docker-compose up --build

 Acesso à API
Após a execução, a API estará disponível em:
http://localhost:8081/swagger/index.html
