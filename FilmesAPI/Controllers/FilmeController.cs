using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{
    // private static List<Filme> filmes = new List<Filme>();
    // private static int id = 0;

    private FilmeContext _context;
    private IMapper _mapper;

    public FilmeController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult AdicinaFilme([FromBody] CreateFilmeDto filmeDto)
    {
        // filme.Id = id++;
        // filmes.Add(filme);

        Filme filme = _mapper.Map<Filme>(filmeDto);

        _context.Filmes.Add(filme);
        _context.SaveChanges();

        // É padrão do rest eu retornar o que foi adicionado após essa adição. No nosso caso, devo retornar o
        // filme para o usuário; e retornar também o caminho que o usuário deve seguir para encontrar o que foi
        // adicionado. Para isso uso o CreatedAtAction(): o 1º parametro é o método usado para retornar o que foi
        // adicionado, logo, método RecuperaFilmePorId(). O 2º parametro é o parametro que esse método anterior
        // precisa para recuperar um filme, ou seja, um ID. Por fim, o 3º parâmetro é o próprio objeto criado, ou
        // seja, no meu caso, um filme. Isso tudo será retornado no final do método. E meu método será do tipo
        // IActionResult.
        return CreatedAtAction(nameof(RecuperaFilmePorId), new { id = filme.Id }, filme);
    }

    [HttpGet]
    public IEnumerable<Filme> RecuperaFilmes([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        return _context.Filmes.Skip(skip).Take(take);

        //return filmes.Skip(skip).Take(take);
    }

    [HttpGet("{id}")]
    public IActionResult RecuperaFilmePorId(int id) 
        // Inicialmente retornava um Filme. Porém para ter o método
        // NotFound() e/ou Ok() preciso retornar um tipo IActionResult, que é uma interface que é um resultado de
        // uma ação que foi executada.
    {

        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);

        // var filme = filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null)
        {
            return NotFound();
        }
        else 
        {
            return Ok(filme);
        }
    }
}
