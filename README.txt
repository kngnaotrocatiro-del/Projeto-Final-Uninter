# trabalhoUninter (ASP.NET Core Web API - .NET 8)

API REST/RESTful com EF Core InMemory, Swagger e autenticação Basic (sobrenome + RU).

## Como usar

1) Instale o .NET SDK 8.
2) Abra um terminal nesta pasta e rode:
   dotnet restore
   dotnet run

3) Abra o Swagger:
   http://localhost:5146/swagger

4) Clique em **Authorize** e use:
   - Username: SEU_SOBRENOME_AQUI
   - Password: SEU_RU_AQUI

5) Endpoints:
   - POST /login (opcional; apenas verifica credenciais)
   - GET /aluno  (seus dados)
   - PUT /aluno  (alterar seus dados)
   - GET /livros
   - GET /livros/{id}
   - POST /livros
   - PUT /livros/{id}
   - DELETE /livros/{id}

## Edite SEUS DADOS
- Em Program.cs: substitua as constantes SOBRENOME e RU.
- Em Models/Aluno.cs ou Data/AppDbContext.cs: ajuste Nome/RU/Curso.
- Para a exigência do exercício: DELETE no livro do LEDUR e POST no livro do MARINHO pelo Swagger.

## Observações
- Este projeto usa Minimal APIs (Program.cs) e EF InMemory (não persiste após reiniciar).
- O Swagger exige Basic Auth em /aluno e /livros.
