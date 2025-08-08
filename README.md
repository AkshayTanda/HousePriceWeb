# House Price Predictor

An end-to-end sample demonstrating how to build, tune, and deploy a house price prediction solution using **ML.NET** and **ASP.NET Core MVC**, complete with interactive console, web UI, batch CSV scoring, Docker containerization, and CI/CD automation.

---

## 🚀 Project Highlights

- **ML.NET Pipeline**: Data loading, feature engineering (concatenate, normalization, custom feature), and regression training with the FastTree algorithm.
- **Hyperparameter Tuning**: Automated grid search over `NumberOfTrees` and `LearningRate` using 5-fold cross-validation to identify the best model.
- **Console App**: Interactive CLI for single-sample predictions and batch CSV scoring, saving the best model (`HousePriceModel_Best.zip`).
- **Web App**: ASP.NET Core MVC site with:
  - **Single-input** form for on-the-fly predictions.
  - **Batch CSV upload** and table view of all scored rows.
  - **Model metrics** (R², RMSE) displayed prominently.
  - **Bootstrap**-styled responsive UI and client-side validation.
  - **Privacy** page with a simple data-handling policy.
- **Docker**: Multi-stage Dockerfile to build and run the MVC app in a lean runtime container. Hosted locally and published to Docker Hub (`atanda01/housepriceweb:latest`).
- **CI/CD**: GitHub Actions workflow that automatically builds & publishes the Docker image on every push to `main`.

---

## 🛠 Tools & Frameworks

- **.NET 9.0 SDK** & **ML.NET**
- **ASP.NET Core MVC**
- **Bootstrap 5** for styling
- **Docker Desktop** & **Docker Hub**
- **GitHub Actions** for CI/CD

All development was done on macOS using VS Code or terminal editors; no paid software required.

---

## 🔧 Setup & Usage

1. **Clone the repo**:

   ```bash
   git clone https://github.com/atanda01/HousePriceWeb.git
   cd HousePriceWeb
   ```

2. **Run locally**:

   ```bash
   dotnet build
   dotnet run
   ```

   Open `https://localhost:5001/` (or the port shown) to access the UI.

3. **Docker**:

   ```bash
   docker build -t housepriceweb:latest .
   docker run -d -p 5054:80 housepriceweb:latest
   ```

   Browse to `http://localhost:5054/`.

4. **CI/CD**: Push changes to `main`; GitHub Actions will rebuild & push to `docker.io/atanda01/housepriceweb:latest`.

---

## 🧠 ML.NET Pipeline Overview

```csharp
// 1. Load data
var data = mlContext.Data.LoadFromTextFile<HouseData>("data/housing.csv", hasHeader: true, separatorChar: ',');

// 2. Feature engineering
var pipeline = mlContext.Transforms
    .Concatenate("Features", nameof(HouseData.Rooms), nameof(HouseData.Area), nameof(HouseData.Age))
    .Append(mlContext.Transforms.NormalizeMinMax("Features"))
    .Append(mlContext.Regression.Trainers.FastTree(options));

// 3. Hyperparameter grid search via 5-fold CV
// 4. Train final model on full data
mlContext.Model.Save(finalModel, data.Schema, "HousePriceModel_Best.zip");
```

---

## ⚠️ Caution

*All predicted values are ****approximate**** and intended for demonstration purposes only. Real-world deployment requires more robust data, validation, and domain-specific adjustments.*

---

## 📄 License

MIT License

Copyright (c) 2025 Akshay Tanda

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

