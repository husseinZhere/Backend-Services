"""
FastAPI server for PulseX AI Service
This service provides endpoints for medical image analysis and OCR for lab tests
"""

from fastapi import FastAPI, File, UploadFile, HTTPException
from fastapi.middleware.cors import CORSMiddleware
from fastapi.responses import JSONResponse
import uvicorn
from typing import List
import os
from pathlib import Path

from services.xray_service import XRayService
from services.ocr_service import OCRService
from services.chatbot_service import ChatbotService
from pydantic import BaseModel

app = FastAPI(
    title="PulseX AI Service",
    description="AI service for medical image analysis and lab test OCR",
    version="1.0.0"
)

# Configure CORS for .NET backend integration
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],  # Configure specific origins in production
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# Initialize services
xray_service = XRayService()
ocr_service = OCRService()
chatbot_service = ChatbotService()

# Create upload directory if it doesn't exist
UPLOAD_DIR = Path("uploads")
UPLOAD_DIR.mkdir(exist_ok=True)


@app.get("/")
async def root():
    """Root endpoint to check if the service is running"""
    return {
        "message": "PulseX AI Service is running",
        "version": "1.0.0",
        "endpoints": {
            "health": "/health",
            "analyze_xray": "/api/xray/analyze",
            "analyze_lab_test": "/api/lab-test/analyze",
            "get_recommendations": "/api/recommendations",
            "chatbot": "/api/chatbot"
        }
    }


@app.get("/health")
async def health_check():
    """Health check endpoint"""
    return {
        "status": "healthy",
        "services": {
            "xray": "active",
            "ocr": "active",
            "chatbot": "active"
        }
    }


# Pydantic models for request validation
class ChatMessage(BaseModel):
    """Chat message request model"""
    message: str
    user_data: dict = None
    
    class Config:
        json_schema_extra = {
            "example": {
                "message": "I have chest pain and shortness of breath",
                "user_data": {
                    "age": 55,
                    "bmi": 28
                }
            }
        }


@app.post("/api/xray/analyze")
async def analyze_xray(file: UploadFile = File(...)):
    """
    Analyze chest X-ray image for heart disease detection
    
    Args:
        file: X-ray image file (JPEG, PNG)
        
    Returns:
        JSON with prediction, confidence, and recommendations
    """
    try:
        # Validate file type
        if not file.content_type.startswith('image/'):
            raise HTTPException(status_code=400, detail="File must be an image")
        
        # Read file content
        contents = await file.read()
        
        # Analyze X-ray
        result = await xray_service.analyze_xray(contents, file.filename)
        
        return JSONResponse(content=result)
    
    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Error analyzing X-ray: {str(e)}")


@app.post("/api/lab-test/analyze")
async def analyze_lab_test(file: UploadFile = File(...)):
    """
    Extract text from lab test image using OCR
    
    Args:
        file: Lab test image file (JPEG, PNG, PDF)
        
    Returns:
        JSON with extracted text and parsed medical values
    """
    try:
        # Read file content
        contents = await file.read()
        
        # Perform OCR
        result = await ocr_service.analyze_lab_test(contents, file.filename)
        
        return JSONResponse(content=result)
    
    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Error analyzing lab test: {str(e)}")


@app.post("/api/recommendations")
async def get_recommendations(
    xray_file: UploadFile = File(None),
    lab_test_file: UploadFile = File(None)
):
    """
    Get comprehensive health recommendations based on X-ray and/or lab tests
    
    Args:
        xray_file: Optional X-ray image
        lab_test_file: Optional lab test image
        
    Returns:
        JSON with comprehensive recommendations
    """
    try:
        results = {}
        recommendations = []
        risk_level = "low"
        
        # Analyze X-ray if provided
        if xray_file:
            xray_contents = await xray_file.read()
            xray_result = await xray_service.analyze_xray(xray_contents, xray_file.filename)
            results["xray"] = xray_result
            recommendations.extend(xray_result.get("recommendations", []))
            
            # Update risk level based on X-ray prediction
            if xray_result.get("prediction") == "abnormal":
                risk_level = "high" if xray_result.get("confidence", 0) > 0.8 else "medium"
        
        # Analyze lab test if provided
        if lab_test_file:
            lab_contents = await lab_test_file.read()
            lab_result = await ocr_service.analyze_lab_test(lab_contents, lab_test_file.filename)
            results["lab_test"] = lab_result
            recommendations.extend(lab_result.get("recommendations", []))
            
            # Update risk level based on lab values
            if lab_result.get("risk_factors"):
                current_risk = lab_result.get("risk_level", "low")
                if current_risk == "high":
                    risk_level = "high"
                elif current_risk == "medium" and risk_level != "high":
                    risk_level = "medium"
        
        # Generate combined recommendations
        combined_recommendations = {
            "risk_level": risk_level,
            "recommendations": list(set(recommendations)),  # Remove duplicates
            "results": results,
            "summary": generate_summary(risk_level, results)
        }
        
        return JSONResponse(content=combined_recommendations)
    
    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Error generating recommendations: {str(e)}")


@app.post("/api/chatbot")
async def chatbot(chat_message: ChatMessage):
    """
    Heart health chatbot with medical intent validation and risk assessment
    
    Args:
        chat_message: ChatMessage object containing user message and optional user data
        
    Returns:
        JSON with chatbot response, risk assessment, and recommendations
    """
    try:
        # Process message through chatbot service
        result = await chatbot_service.process_message(
            chat_message.message,
            chat_message.user_data
        )
        
        return JSONResponse(content=result)
    
    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Error processing chat message: {str(e)}")


def generate_summary(risk_level: str, results: dict) -> str:
    """Generate a summary based on analysis results"""
    if risk_level == "high":
        return "High risk detected. Please consult with your doctor immediately for detailed examination."
    elif risk_level == "medium":
        return "Moderate risk detected. We recommend scheduling an appointment with your doctor for further evaluation."
    else:
        return "Low risk detected. Continue maintaining healthy lifestyle habits and regular check-ups."


if __name__ == "__main__":
    uvicorn.run(
        "main:app",
        host="0.0.0.0",
        port=8000,
        reload=True
    )
