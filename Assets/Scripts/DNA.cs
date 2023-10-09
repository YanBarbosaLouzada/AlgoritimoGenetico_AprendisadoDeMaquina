using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Esta é a classe que representa o "DNA" de um inimigo, que contém informações genéticas.
[System.Serializable]
public class DNA
{
    public List<int> genes = new List<int>();  // Aqui armazenamos os "genes" do inimigo.
    public List<int> parent1Genes;  // Genes do primeiro pai.
    public List<int> parent2Genes;  // Genes do segundo pai.

    public int maxGeneNumber = 360;  // O valor máximo para cada gene.
    public int geneQuantity = 3;  // Quantidade de genes.

    // Este é o construtor que cria um novo "DNA" com valores iniciais.
    public DNA(int maxGeneNumber, int geneQuantity)
    {
        this.maxGeneNumber = maxGeneNumber;  // Define o valor máximo para os genes.
        this.geneQuantity = geneQuantity;  // Define a quantidade de genes.
        SetGeneValues();  // Inicializa os valores dos genes.

    }

    // Este método inicializa os valores dos genes com números aleatórios.
    void SetGeneValues()
    {
        genes.Clear();  // Limpa a lista de genes.
        for (int i = 0; i < geneQuantity; i++)
        {
            genes.Add(Random.Range(0, maxGeneNumber + 1));  // Adiciona um gene aleatório à lista.
        }
    }

    // Este método combina os genes de dois pais para criar um novo conjunto de genes.
    public void CombineGenes(DNA parent1, DNA parent2)
    {
        parent1Genes = parent1.genes;  // Obtém os genes do primeiro pai.
        parent2Genes = parent2.genes;  // Obtém os genes do segundo pai.

        for (int i = 0; i < geneQuantity; i++)
        {
            if (Random.Range(0, 10) > 5)
                genes[i] = parent1.genes[i];  // Às vezes, escolhe os genes do primeiro pai.
            else
                genes[i] = parent2.genes[i];  // Às vezes, escolhe os genes do segundo pai.
        }
    }

    // Este método causa uma mutação em um dos genes, tornando-o aleatório.
    public void Mutate()
    {
        genes[Random.Range(0, geneQuantity)] = Random.Range(0, maxGeneNumber);
    }

    // Este método obtém o valor de um gene específico pelo índice.
    public int GetGene(int index)
    {
        return genes[index];
    }
}
