from fastapi import FastAPI, UploadFile, File, HTTPException, Request
from fastapi.middleware.cors import CORSMiddleware
from fastapi.responses import JSONResponse
from pathlib import Path
import shutil
import uuid
from datetime import datetime
import os
import sys
from pydantic import BaseModel
from typing import Optional, Dict, Any

sys.path.append(os.path.dirname(os.path.abspath(__file__)))

app = FastAPI(
    title="PulseX AI Service",
    description="Medical AI Service: X-ray Binary Classifier + Heart Health Chatbot",
    version="3.0.0"
)

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# Setup directories
BASE_DIR = Path(__file__).parent
UPLOAD_DIR = BASE_DIR / "uploads"
ECG_DIR = UPLOAD_DIR / "ecg"
XRAY_TEMP_DIR = UPLOAD_DIR / "xray_temp"

for folder in [ECG_DIR, XRAY_TEMP_DIR]:
    folder.mkdir(parents=True, exist_ok=True)

# Initialize services
try:
    from services.xray_service import XRayService
    xray_service = XRayService()
    print("✅ X-ray Binary Classifier loaded (92.6% accuracy)")
except Exception as e:
    print(f"❌ X-ray Service Error: {e}")
    xray_service = None

try:
    from services.chatbot_service import ChatbotService
    chatbot_service = ChatbotService()
    print("✅ Heart Health Chatbot loaded")
except Exception as e:
    print(f"❌ Chatbot Service Error: {e}")
    chatbot_service = None

# Pydantic models for chatbot
class ChatbotRequest(BaseModel):
    message: str
    user_data: Optional[Dict[str, Any]] = None


# ENDPOINTS
@app.get("/", tags=["System"])
async def root():
    return {
        "service": "PulseX AI Service",
        "status": "operational",
        "version": "3.0.0",
        "features": {
            "xray_binary_classifier": "92.6% accuracy (Normal vs Abnormal)",
            "heart_health_chatbot": "Medical intent validation + Risk assessment",
            "ecg_storage": "Secure file upload"
        }
    }

@app.get("/health", tags=["System"])
async def health_check():
    return {
        "status": "healthy",
        "timestamp": datetime.now().isoformat(),
        "services": {
            "xray_binary": "active" if xray_service else "inactive",
            "chatbot": "active" if chatbot_service else "inactive",
            "ecg_storage": "active"
        }
    }


@app.post("/api/xray/analyze", tags=["X-ray Analysis"])
async def analyze_xray(file: UploadFile = File(...)):
    """
    Binary X-ray Classification: Normal vs Abnormal (92.6% accuracy)
    Replaces old DenseNet121 multi-class classifier
    """
    if not xray_service:
        raise HTTPException(status_code=503, detail="X-ray service not initialized")
    
    if not file.content_type.startswith('image/'):
        raise HTTPException(status_code=400, detail="Invalid file type")

    file_id = str(uuid.uuid4())
    file_ext = Path(file.filename).suffix.lower()
    temp_path = XRAY_TEMP_DIR / f"{file_id}{file_ext}"

    try:
        await file.seek(0)
        image_bytes = await file.read()
        
        with open(temp_path, "wb") as buffer:
            buffer.write(image_bytes)
        
        # Run synchronous ML inference in thread pool to avoid blocking event loop
        import asyncio
        result = await asyncio.to_thread(xray_service.analyze_xray, image_bytes, file.filename)
        
        if temp_path.exists():
            temp_path.unlink()

        return {
            "success": True,
            "result": result,
            "timestamp": datetime.now().isoformat()
        }
    
    except Exception as e:
        if temp_path.exists(): 
            temp_path.unlink()
        raise HTTPException(status_code=500, detail=f"Analysis Error: {str(e)}")


@app.post("/api/ecg/upload", tags=["ECG Storage"])
async def upload_ecg(file: UploadFile = File(...)):
    """Upload ECG file for secure storage"""
    allowed_extensions = {'.png', '.jpg', '.jpeg', '.pdf'}
    file_ext = Path(file.filename).suffix.lower()
    
    if file_ext not in allowed_extensions:
        raise HTTPException(status_code=400, detail="Only PNG, JPG, JPEG, PDF allowed")

    file_id = str(uuid.uuid4())
    timestamp = datetime.now().strftime("%Y%m%d_%H%M%S")
    unique_filename = f"ecg_{timestamp}_{file_id}{file_ext}"
    save_path = ECG_DIR / unique_filename

    try:
        with open(save_path, "wb") as buffer:
            shutil.copyfileobj(file.file, buffer)
        
        return {
            "success": True,
            "message": "ECG uploaded successfully",
            "file_info": {
                "filename": unique_filename,
                "path": f"uploads/ecg/{unique_filename}",
                "size_mb": round(save_path.stat().st_size / (1024 * 1024), 2),
                "timestamp": datetime.now().isoformat()
            }
        }
    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Upload Error: {str(e)}")


@app.post("/api/chatbot", tags=["Heart Health Chatbot"])
async def chatbot(request: ChatbotRequest):
    """
    Heart Health Chatbot with Medical Intent Validation
    KEPT FROM ORIGINAL REPO - NO CHANGES
    """
    if not chatbot_service:
        raise HTTPException(status_code=503, detail="Chatbot service not initialized")
    
    try:
        result = await chatbot_service.process_message(
            message=request.message,
            user_data=request.user_data or {}
        )
        return result
    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Chatbot Error: {str(e)}")

@app.exception_handler(404)
async def not_found(request: Request, exc):
    return JSONResponse(
        status_code=404,
        content={
            "error": "Not Found",
            "available": ["/api/xray/analyze", "/api/ecg/upload", "/api/chatbot"]
        }
    )

@app.on_event("startup")
async def startup():
    print("=" * 70)
    print("PulseX AI Service v3.0.0")
    print("=" * 70)
    print(f"✓ X-ray Binary Classifier: {'Active' if xray_service else 'Inactive'}")
    print(f"✓ Heart Health Chatbot: {'Active' if chatbot_service else 'Inactive'}")
    print(f"✓ ECG Storage: Active")
    print("=" * 70)

if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8000)
