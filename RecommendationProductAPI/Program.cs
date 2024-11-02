using Microsoft.ML;
using RecommendationProductAPI.Model;

var builder = WebApplication.CreateBuilder(args);

var mlContext = new MLContext();

var dataPath = "products.csv";
IDataView dataView = mlContext.Data.LoadFromTextFile<Product>(dataPath, separatorChar: ',', hasHeader: true);

// Pipeline de processamento de dados e treinamento
var pipeline = mlContext.Transforms.Conversion.MapValueToKey("Label", nameof(Product.Name))
    .Append(mlContext.Transforms.Text.FeaturizeText("Features", nameof(Product.Description)))
    .Append(mlContext.Transforms.Concatenate("Features", "Category", "Mark", "Price", "Description"))
    .Append(mlContext.Recommendation().Trainers.MatrixFactorization("Label", "Features"));

var model = pipeline.Fit(dataView);

// Save o modelo para o uso em uma API
mlContext.Model.Save(model, dataView.Schema, "modelo_recomendacao.zip");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
