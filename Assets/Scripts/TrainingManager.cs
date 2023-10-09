using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Esta classe gerencia o treinamento de inimigos em um jogo.
public class TrainingManager : MonoBehaviour
{
    public GameObject prefab;  // O modelo do inimigo.
    public Vector3 startingPosition;  // A posição inicial para os inimigos.
    public int populationSize;  // O tamanho da população de inimigos.
    public int generation;  // A geração atual.
    public float trainingTime;  // O tempo de treinamento para cada geração.
    public float elapsedTime;  // O tempo que passou desde o início do treinamento.
    public List<BaseEnemy> population = new List<BaseEnemy>();  // Lista de inimigos.

    void Start()
    {
        InitializePopulation();  // Inicializa a população de inimigos.
    }

    void InitializePopulation()
    {
        for (int i = 0; i < populationSize; i++)
        {
            // Cria um novo inimigo a partir do modelo (prefab).
            BaseEnemy e = Instantiate(prefab, startingPosition, Quaternion.identity).GetComponent<BaseEnemy>();
            e.Initialize(e);  // Inicializa o inimigo.
            population.Add(e);  // Adiciona o inimigo à população.
        }
    }

    // Cruzar dois inimigos para criar um novo.
    BaseEnemy Breed(BaseEnemy parent1, BaseEnemy parent2)
    {
        BaseEnemy offspring = Instantiate(prefab, startingPosition, Quaternion.identity).GetComponent<BaseEnemy>();
        offspring.Initialize(offspring);  // Inicializa o novo inimigo.
        offspring.brain = new BaseEnemy.Brain();  // Cria um novo "cérebro" para o inimigo.
        offspring.dna.CombineGenes(parent1.dna, parent2.dna);  // Combina os genes dos pais.
        return offspring;
    }

    // Cruzar a população para criar uma nova geração.
    void BreedPopulation()
    {
        List<BaseEnemy> sortedList = population.OrderByDescending(x => x.brain.timeAlive).ToList();
        List<BaseEnemy> newGeneration = new List<BaseEnemy>();

        for (int i = 0; i < sortedList.Count - 1; i += 2)
        {   
            // Cria dois novos inimigos a partir de pares da geração anterior.
            newGeneration.Add(Breed(sortedList[i], sortedList[i + 1]));
            newGeneration.Add(Breed(sortedList[i + 1], sortedList[i]));
        }

        // Destroi os inimigos da geração anterior.
        foreach (BaseEnemy enemy in population)
        {
            Destroy(enemy.gameObject);
        }

        population = newGeneration;  // Define a nova geração como a população atual.
        generation++;  // Incrementa o número da geração.
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        
        // Verifica se o tempo de treinamento para a geração atual foi atingido.
        if (elapsedTime > trainingTime)
        {
            BreedPopulation();  // Cria uma nova geração de inimigos.
            elapsedTime = 0;  // Reinicia o contador de tempo.
        }
    }
}
