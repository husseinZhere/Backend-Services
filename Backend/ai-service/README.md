# PulseX AI Service

AI service for medical image analysis and lab test OCR in the PulseX healthcare platform.

## Overview

This service provides AI-powered analysis for:
- **Chest X-ray Analysis**: Using DenseNet121 pre-trained model for heart disease detection
- **Lab Test OCR**: Using EasyOCR to extract and analyze medical test values
- **Heart Health Chatbot**: Intelligent chatbot with medical intent validation and risk assessment
- **Health Recommendations**: Smart AI-generated recommendations based on analysis results

## Features

✅ **X-ray Analysis**
- Chest X-ray image classification (Normal/Abnormal)
- Confidence score for predictions
- Risk level assessment
- Personalized recommendations

✅ **Lab Test Analysis**
- OCR text extraction from lab test images
- Automatic parsing of medical values (cholesterol, blood pressure, blood sugar, etc.)
- Risk factor identification
- Health metric analysis with normal range comparison

✅ **Combined Analysis**
- Comprehensive health assessment combining X-ray and lab tests
- Unified risk level calculation
- Smart recommendations based on multiple data sources

✅ **Heart Health Chatbot**
- Natural language processing for symptom analysis
- Medical intent validation with guardrails
- Heart-health domain specialization
- Real-time risk assessment based on symptoms
- Personalized health recommendations
- Multi-level risk categorization (Low, Medium, High)

## Project Structure

```
ai-service/
├── main.py                 # FastAPI server
├── services/
│   ├── xray_service.py    # X-ray analysis service (DenseNet121)
│   ├── ocr_service.py     # Lab test OCR service (EasyOCR)
│   └── chatbot_service.py # Heart health chatbot service
├── models/                 # Pre-trained models directory
├── test_images/           # Test images for development
├── uploads/               # Temporary upload directory
├── requirements.txt       # Python dependencies
├── .gitignore            # Git ignore rules
└── README.md             # This file
```

## Installation

### Prerequisites

- Python 3.8 or higher
- pip package manager
- Virtual environment (recommended)

### Setup Steps

1. **Clone the repository**
   ```bash
   cd /path/to/PulseX
   cd ai-service
   ```

2. **Create and activate virtual environment**
   ```bash
   # Create virtual environment
   python -m venv venv

   # Activate (Linux/Mac)
   source venv/bin/activate

   # Activate (Windows)
   venv\Scripts\activate
   ```

3. **Install dependencies**
   ```bash
   pip install -r requirements.txt
   ```

   Note: The first run will download pre-trained models:
   - DenseNet121 (~30MB)
   - EasyOCR language models (~100MB)

## Usage

### Starting the Server

```bash
# Development mode with auto-reload
python main.py

# Or using uvicorn directly
uvicorn main:app --reload --host 0.0.0.0 --port 8000
```

The server will start at `http://localhost:8000`

### API Endpoints

#### 1. Health Check
```bash
GET /health
```

Response:
```json
{
  "status": "healthy",
  "services": {
    "xray": "active",
    "ocr": "active"
  }
}
```

#### 2. Analyze X-ray
```bash
POST /api/xray/analyze
Content-Type: multipart/form-data
```

Parameters:
- `file`: X-ray image file (JPEG, PNG)

Response:
```json
{
  "filename": "chest_xray.jpg",
  "prediction": "normal",
  "confidence": 0.8523,
  "probabilities": {
    "normal": 0.8523,
    "abnormal": 0.1477
  },
  "risk_level": "low",
  "recommendations": [
    "✅ Chest X-ray appears normal",
    "💚 Continue maintaining healthy lifestyle habits",
    ...
  ]
}
```

#### 3. Analyze Lab Test
```bash
POST /api/lab-test/analyze
Content-Type: multipart/form-data
```

Parameters:
- `file`: Lab test image file (JPEG, PNG, PDF)

Response:
```json
{
  "filename": "lab_test.jpg",
  "extracted_text": "Total Cholesterol: 185 mg/dL\nLDL: 95 mg/dL\n...",
  "parsed_values": {
    "total_cholesterol": 185,
    "ldl": 95,
    "hdl": 55,
    "blood_pressure": {
      "systolic": 118,
      "diastolic": 75
    }
  },
  "analysis": {
    "normal": ["Total Cholesterol: 185 mg/dL (Normal)", ...],
    "abnormal": [],
    "risk_factors": [],
    "risk_level": "low"
  },
  "recommendations": [...],
  "confidence": 0.9234
}
```

#### 4. Get Comprehensive Recommendations
```bash
POST /api/recommendations
Content-Type: multipart/form-data
```

Parameters:
- `xray_file`: (Optional) X-ray image
- `lab_test_file`: (Optional) Lab test image

Response:
```json
{
  "risk_level": "low",
  "recommendations": [...],
  "results": {
    "xray": {...},
    "lab_test": {...}
  },
  "summary": "Low risk detected. Continue maintaining healthy lifestyle habits..."
}
```

#### 5. Heart Health Chatbot
```bash
POST /api/chatbot
Content-Type: application/json
```

Parameters (JSON body):
```json
{
  "message": "I have chest pain and shortness of breath",
  "user_data": {
    "age": 55,
    "bmi": 28
  }
}
```

Response:
```json
{
  "success": true,
  "response": "# Heart Health Assessment Results\n\n## Risk Level: High Risk ⚠️\n...",
  "type": "risk_assessment",
  "risk_level": "high",
  "risk_score": 0.78,
  "symptoms": ["chest pain", "shortness of breath"],
  "recommendations": [
    "🚨 URGENT: Please seek immediate medical attention",
    "📞 Contact your doctor or go to the emergency room",
    ...
  ],
  "requires_clarification": false
}
```

**Chatbot Features:**
- **Medical Intent Validation**: Ensures queries are medically relevant
- **Domain Specialization**: Focuses specifically on heart health
- **Symptom Extraction**: Automatically identifies heart-related symptoms
- **Risk Assessment**: Calculates cardiovascular risk based on symptoms and user data
- **Smart Recommendations**: Provides personalized advice based on risk level

**Chatbot Response Types:**
- `non_medical`: User query is not medical-related
- `non_heart_medical`: Medical query but not heart-related
- `needs_more_info`: Heart-related but needs more symptom details
- `risk_assessment`: Full risk assessment with recommendations
- `error`: An error occurred during processing

### Testing with cURL

**X-ray Analysis:**
```bash
curl -X POST "http://localhost:8000/api/xray/analyze" \
  -H "accept: application/json" \
  -F "file=@test_images/chest_xray.jpg"
```

**Lab Test Analysis:**
```bash
curl -X POST "http://localhost:8000/api/lab-test/analyze" \
  -H "accept: application/json" \
  -F "file=@test_images/lab_test.jpg"
```

**Combined Analysis:**
```bash
curl -X POST "http://localhost:8000/api/recommendations" \
  -H "accept: application/json" \
  -F "xray_file=@test_images/chest_xray.jpg" \
  -F "lab_test_file=@test_images/lab_test.jpg"
```

**Heart Health Chatbot:**
```bash
curl -X POST "http://localhost:8000/api/chatbot" \
  -H "accept: application/json" \
  -H "Content-Type: application/json" \
  -d '{
    "message": "I have chest pain and high blood pressure",
    "user_data": {
      "age": 60,
      "bmi": 32
    }
  }'
```

### Interactive API Documentation

Once the server is running, access:
- **Swagger UI**: http://localhost:8000/docs
- **ReDoc**: http://localhost:8000/redoc

## Integration with .NET Backend

### Connection Setup

The FastAPI service is designed to integrate seamlessly with the .NET backend:

1. **CORS Configuration**: Already configured to accept requests from any origin (configure specific origins in production)

2. **REST API**: Standard REST endpoints compatible with .NET HttpClient

3. **JSON Responses**: All responses are in JSON format

### .NET Integration Example

```csharp
using System.Net.Http;
using System.Net.Http.Headers;

public class AIServiceClient
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "http://localhost:8000";

    public AIServiceClient()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
    }

    public async Task<XRayAnalysisResult> AnalyzeXRayAsync(byte[] imageBytes, string fileName)
    {
        using var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(imageBytes);
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
        content.Add(fileContent, "file", fileName);

        var response = await _httpClient.PostAsync("/api/xray/analyze", content);
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadAsAsync<XRayAnalysisResult>();
    }

    public async Task<LabTestAnalysisResult> AnalyzeLabTestAsync(byte[] imageBytes, string fileName)
    {
        using var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(imageBytes);
        content.Add(fileContent, "file", fileName);

        var response = await _httpClient.PostAsync("/api/lab-test/analyze", content);
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadAsAsync<LabTestAnalysisResult>();
    }
}
```

## Datasets

This service is designed to work with the following datasets:

1. **Cardiovascular Disease Dataset**
   - URL: https://www.kaggle.com/datasets/sulianova/cardiovascular-disease-dataset
   - Use: Training cardiovascular risk assessment models

2. **Chest X-ray Masks and Labels**
   - URL: https://www.kaggle.com/datasets/nikhilpandey360/chest-xray-masks-and-labels
   - Use: Fine-tuning X-ray classification models

## Model Information

### DenseNet121 (X-ray Analysis)
- **Architecture**: DenseNet121 pre-trained on ImageNet
- **Task**: Binary classification (Normal/Abnormal)
- **Input Size**: 224x224 RGB images
- **Output**: 2 classes with probability scores

### EasyOCR (Lab Test Analysis)
- **Languages**: English
- **Capabilities**: Text extraction from medical documents
- **Post-processing**: Medical value parsing and analysis

## Performance

- **X-ray Analysis**: ~1-2 seconds per image (CPU)
- **Lab Test OCR**: ~2-3 seconds per image (CPU)
- **Combined Analysis**: ~3-5 seconds (CPU)

GPU acceleration can significantly improve performance.

## Development

### Running Tests

```bash
# Add test files to test_images directory
# Run the server and test endpoints using the interactive docs
```

### Adding New Features

1. Create new service files in `services/` directory
2. Import and initialize in `main.py`
3. Add new endpoints in `main.py`
4. Update `requirements.txt` if needed

## Production Deployment

### Recommendations

1. **Use GPU**: Enable GPU support for faster inference
   ```python
   # In xray_service.py and ocr_service.py
   self.device = torch.device("cuda" if torch.cuda.is_available() else "cpu")
   self.reader = easyocr.Reader(['en'], gpu=True)  # Enable GPU for OCR
   ```

2. **Configure CORS**: Set specific allowed origins
   ```python
   app.add_middleware(
       CORSMiddleware,
       allow_origins=["https://your-frontend-domain.com"],
       ...
   )
   ```

3. **Use Environment Variables**: Store configuration in `.env` file
   ```bash
   PORT=8000
   HOST=0.0.0.0
   MAX_FILE_SIZE=10485760  # 10MB
   ```

4. **Add Authentication**: Implement API key or JWT authentication

5. **Add Rate Limiting**: Prevent abuse

6. **Use Production Server**: Deploy with Gunicorn + Uvicorn workers
   ```bash
   gunicorn main:app --workers 4 --worker-class uvicorn.workers.UvicornWorker --bind 0.0.0.0:8000
   ```

## Troubleshooting

### Common Issues

1. **Module not found errors**
   - Ensure virtual environment is activated
   - Run `pip install -r requirements.txt` again

2. **CUDA errors**
   - If no GPU available, models will automatically use CPU
   - Check CUDA installation if GPU is available but not detected

3. **EasyOCR download issues**
   - First run downloads language models
   - Ensure stable internet connection
   - Models cached in `~/.EasyOCR/`

4. **Memory errors**
   - Reduce batch size or image resolution
   - Use GPU if available
   - Increase system RAM

## License

This service is part of the PulseX project.

## Support

For issues or questions, please contact the development team.
