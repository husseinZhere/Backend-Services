import React, { useState } from 'react';
import { Check, ChevronRight, ChevronLeft, BadgeCheck } from 'lucide-react'; // تم إزالة Quote
import Container from '../ui/Container';
import SectionHeader from '../ui/SectionHeader';

// الصور المُضافة حديثًا:
import Qpute from '../../assets/quotation-mark_stores.svg';
import verfication from '../../assets/ic_round-verified-user_story.svg';

// مسارات الصور المُعرّفة:
import patientImg1 from '../../assets/PulseX - Graduation Project File_img/source/Portrait of an Older Woman.png';
import patientImg2 from '../../assets/PulseX - Graduation Project File_img/source/Joyful Elderly Man in Garden.png';
import patientImg3 from '../../assets/PulseX - Graduation Project File_img/source/Serene Professional Portrait.png';
import patientImg4 from '../../assets/PulseX - Graduation Project File_img/source/Warm and Stylish Latino with Beard.png';

// مصفوفة الصور لسهولة الوصول إليها بالفهرس
const PATIENT_IMAGES = [patientImg1, patientImg2, patientImg3, patientImg4];

// تعريف الألوان كمتغيرات ثابتة
const COLORS = {
    BlackMainText: '#010218',
    Blue1: '#333CF5',
    SecondTextColor: '#757575',
    EmeraldGreen: '#059669',
    AmberOrange: '#D97706',
    LightGreyBg: '#D9D9D9',
    OutlineGrey: 'rgba(115, 115, 115, 0.5)', 
};

// بيانات القصص (كما هي)
const STORIES = [
    {
        id: 1,
        name: "Ghada Ali",
        age: "70",
        condition: "Hypertension",
        quote: "\"After my heart attack scare, Pulse AI became my lifeline. The AI risk assessment showed I was at high risk, but with the personalized recommendations and regular doctor follow-ups, I've reduced my risk score from 85% to just 23% in 8 months. The medication reminders and emergency QR code give me peace of mind every day.\"",
        stats: { risk: "-62%", time: "8 months", bpm: "-15 BPM" },
        progress: 62,
        tags: ["High Risk", "Recovery", "Lifestyle Change"]
    },
    {
        id: 2,
        name: "Ahmed Hassan",
        age: "55",
        condition: "Arrhythmia",
        quote: "\"I never realized how irregular my heartbeat was until I used the ECG feature. The instant alerts helped me seek medical attention right before a critical episode. PulseX truly saved my life.\"",
        stats: { risk: "-45%", time: "6 months", bpm: "Stable" },
        progress: 85,
        tags: ["Early Detection", "Monitoring", "Medication"]
    },
    {
        id: 3,
        name: "Layla Mahmoud",
        age: "42",
        condition: "Post-Surgery",
        quote: "\"Recovering from heart surgery was daunted, but the daily check-ins and direct connection to my doctor gave me confidence. I felt safe knowing my vitals were being monitored 24/7.\"",
        stats: { risk: "-30%", time: "4 months", bpm: "-10 BPM" },
        progress: 40,
        tags: ["Post-Op", "Care Plan", "Virtual Visits"]
    },
    {
        id: 4,
        name: "Samir Nabil",
        age: "48",
        condition: "High Cholesterol",
        quote: "\"The lifestyle recommendations were a game changer. I didn't just take pills; I changed my habits based on the AI insights. My cholesterol levels dropped significantly without heavy medication.\"",
        stats: { risk: "-50%", time: "1 year", bpm: "-5 BPM" },
        progress: 90,
        tags: ["Prevention", "Diet", "Active Life"]
    }
];

const RecoveryStories = () => {
    const [currentIndex, setCurrentIndex] = useState(0);

    const nextSlide = () => {
        setCurrentIndex(prev => (prev < STORIES.length - 1 ? prev + 1 : prev));
    };

    const prevSlide = () => {
        setCurrentIndex(prev => (prev > 0 ? prev - 1 : prev));
    };

    const cardShadow = 'shadow-[0px_0px_3px_0px_rgba(0,0,0,0.08)] shadow-[0px_2px_3px_0px_rgba(0,0,0,0.17)]';

    return (
        <section id="stories" className="py-24 bg-white">
            <Container>
                {/* Header Section */}
                <SectionHeader 
                    title="Recovery Stories" 
                    subtitle="Real patients, real results - inspiring journeys to better heart health" 
                />

                {/* Main Slider Container - العرض ثابت 1000px */}
                <div className="relative w-[1000px] mx-auto">
                    
                    {/* Story Details Card - الحجم الدقيق 1000x473px */}
                    <div 
                        className={`w-[1000px] h-[503px] overflow-hidden rounded-2xl ${cardShadow} bg-white relative z-10`}
                        style={{ outline: `0.20px solid ${COLORS.OutlineGrey}`, outlineOffset: '-0.20px' }}
                    >
                        
                        <div 
                            className="flex h-full transition-transform duration-500 ease-in-out"
                            style={{ transform: `translateX(-${currentIndex * 100}%)` }}
                        >
                            {STORIES.map((story, idx) => (
                                <div key={story.id} className="w-full flex-shrink-0 relative h-full">
                                    {/* 1. Patient Details - يسار البطاقة */}
                                    <div 
                                        className="w-60 absolute top-[90px] flex flex-col items-center gap-6"
                                        style={{ left: '82.99px' }}
                                    >
                                        <div className="relative mb-6">
                                            <img 
                                                src={PATIENT_IMAGES[idx]} 
                                                alt={story.name} 
                                                className="size-36 rounded-full object-cover shadow-lg" 
                                            />
                                            <div 
                                                className="absolute bottom-1 right-1 size-6 rounded-full border-[3px] border-white flex items-center justify-center text-white"
                                                style={{ backgroundColor: COLORS.EmeraldGreen }}
                                            >
                                                {/* تم استخدام أيقونة Check من lucide-react هنا لأنها جزء من الصورة الدائرية */}
                                                <Check className="w-3 h-3" strokeWidth={4} /> 
                                            </div>
                                        </div>
                                        
                                        <div className="w-full flex flex-col justify-start items-center gap-6 text-center">
                                            <div className="flex flex-col justify-start items-center gap-2">
                                                <h3 className="text-xl font-bold" style={{ color: COLORS.BlackMainText }}>{story.name}</h3>
                                                <p className="text-base font-normal" style={{ color: COLORS.SecondTextColor }}>Age {story.age}</p>
                                                <p className="text-base font-normal" style={{ color: COLORS.Blue1 }}>{story.condition}</p>
                                            </div>

                                            <div className="flex justify-center items-center gap-4">
                                                {story.tags.map((tag, tagIdx) => (
                                                    <span key={tagIdx} className="text-xs font-normal" style={{ color: COLORS.Blue1 }}>
                                                        {tag}
                                                    </span>
                                                ))}
                                            </div>
                                        </div>
                                    </div>

                                    {/* 2. Story Disc - يمين البطاقة */}
                                    <div 
                                        className="absolute w-[452px] flex flex-col justify-start items-center gap-10"
                                        style={{ left: '464.99px', top: '40px' }}
                                    >
                                        <div 
                                            className="absolute size-6 overflow-hidden top-0"
                                            style={{ left: '-44px' }}
                                        >
                                            {/* **التعديل هنا:** استخدام صورة SVG Qpute بدلاً من أيقونة Quote */}
                                            <img 
                                                src={Qpute} 
                                                alt="Quote Icon" 
                                                className="size-6 rotate-180" 
                                                style={{ filter: `drop-shadow(0 0 0 ${COLORS.Blue1})` }} // للحفاظ على لون الأيقونة إذا كانت SVG بيضاء
                                            />
                                        </div>

                                        <p 
                                            className="self-stretch text-lg font-normal font-['Inter']" 
                                            style={{ color: COLORS.BlackMainText }}
                                        >
                                            {story.quote}
                                        </p>

                                        {/* STATS BAR */}
                                        <div className="self-stretch inline-flex justify-start items-end gap-11">
                                            
                                            {/* Risk Reduction */}
                                            <div className="w-28 flex flex-col justify-start items-center gap-2"> 
                                                <p className="self-stretch text-center text-2xl font-bold" style={{ color: COLORS.EmeraldGreen }}>{story.stats.risk}</p>
                                                <p className="self-stretch text-center text-sm font-normal whitespace-nowrap" style={{ color: COLORS.SecondTextColor }}>Risk Reduction</p>
                                            </div>

                                            {/* Recovery Time */}
                                            <div className="w-32 flex flex-col justify-start items-center gap-2"> 
                                                <p className="self-stretch text-center text-2xl font-bold" style={{ color: COLORS.Blue1 }}>{story.stats.time}</p>
                                                <p className="self-stretch text-center text-sm font-normal whitespace-nowrap" style={{ color: COLORS.SecondTextColor }}>Recovery Time</p>
                                            </div>

                                            {/* BPM Improved */}
                                            <div className="w-32 flex flex-col justify-start items-center gap-2"> 
                                                <p className="self-stretch text-center text-2xl font-bold" style={{ color: COLORS.AmberOrange }}>{story.stats.bpm}</p>
                                                <p className="self-stretch text-center text-sm font-normal whitespace-nowrap" style={{ color: COLORS.SecondTextColor }}>BPM Improved</p>
                                            </div>
                                        </div>
                                    </div>

                                    {/* 3. Recovery Progress Bar and Verified Story */}
                                    <div 
                                        className="absolute w-[499px] flex flex-col justify-start items-center gap-4"
                                        style={{ left: '414.99px', top: '360px' }} 
                                    >
                                        {/* Progress Header */}
                                        <div className="self-stretch flex flex-col justify-start items-center gap-2">
                                            <div className="w-full inline-flex justify-between items-center" style={{ width: '487.96px' }}>
                                                <span className="text-sm font-normal" style={{ color: COLORS.SecondTextColor }}>Recovery Progress</span>
                                                <span className="text-sm font-medium" style={{ color: COLORS.BlackMainText }}>{story.progress}%</span>
                                            </div>
                                        </div>

                                        {/* Progress Bar - الارتفاع 12px (h-3) */}
                                        <div className="w-[499px] h-6 relative overflow-hidden"> {/* تم إزالة mb-2 */}
                                            <div 
                                                className="w-full h-3 absolute top-[5.75px] rounded-[10px]" 
                                                style={{ backgroundColor: COLORS.LightGreyBg, left: '5.33px', width: '488.35px' }}
                                            />
                                            <div 
                                                className="h-3 absolute top-[5.75px] rounded-[10px] transition-all duration-500"
                                                style={{ 
                                                    left: '5.32px',
                                                    width: `${story.progress * 4.88}px`, 
                                                    background: `linear-gradient(to right, ${COLORS.EmeraldGreen}, #2564EB)` 
                                                }}
                                            ></div>
                                        </div>
                                        
                                        {/* Separator Line */}
                                        <div 
                                            className="w-full" // تم إزالة mb-4
                                            style={{ height: '0.20px', backgroundColor: COLORS.OutlineGrey }}
                                        />

                                        {/* Verified Patient Story - **التعديل هنا** */}
                                        <div 
                                            className="flex justify-start items-center gap-2 w-full"
                                            // استخدام محاذاة دقيقة ليتطابق مع الصورة الجديدة
                                            style={{ marginTop: '5px', marginLeft: '5px' }} 
                                        >
                                            {/* **التعديل هنا:** استخدام صورة SVG verfication بدلاً من Check */}
                                            <img 
                                                src={verfication} 
                                                alt="Verified" 
                                                className="size-4" 
                                            />
                                            <span 
                                                className="text-sm font-normal" 
                                                style={{ color: COLORS.SecondTextColor }}
                                            >
                                                Verified Patient Story
                                            </span>
                                        </div>
                                    </div>
                                    
                                </div>
                            ))}
                        </div>
                    </div>

                    {/* Navigation Arrows */}
                    {currentIndex < STORIES.length - 1 && (
                        <button 
                            onClick={nextSlide}
                            className="absolute top-1/2 -translate-y-1/2 right-[-74px] size-12 bg-white rounded-full shadow-md border border-gray-100 flex items-center justify-center hover:bg-gray-50 transition-colors z-20 group"
                        >
                            <ChevronRight className="w-6 h-6" style={{ color: COLORS.BlackMainText, strokeWidth: 1.5 }} />
                        </button>
                    )}

                    {currentIndex > 0 && (
                        <button 
                            onClick={prevSlide}
                            className="absolute top-1/2 -translate-y-1/2 left-[-74px] size-12 bg-white rounded-full shadow-md border border-gray-100 flex items-center justify-center hover:bg-gray-50 transition-colors z-20 group"
                        >
                            <ChevronLeft className="w-6 h-6" style={{ color: COLORS.BlackMainText, strokeWidth: 1.5 }} />
                        </button>
                    )}

                </div>

                {/* Pagination Dots */}
                <div className="flex flex-col items-center justify-center mt-12 gap-3">
                    <div className="flex gap-2">
                        {STORIES.map((_, index) => (
                            <div 
                                key={index}
                                className={`w-10 h-2.5 rounded-full transition-all duration-300`}
                                style={{ 
                                    backgroundColor: currentIndex === index ? COLORS.Blue1 : COLORS.LightGreyBg
                                }}
                            ></div>
                        ))}
                    </div>
                    <p className="text-[13px] font-medium" style={{ color: COLORS.SecondTextColor }}>
                        {currentIndex + 1} of {STORIES.length} stories
                    </p>
                </div>
            </Container>
        </section>
    );
};

export default RecoveryStories;