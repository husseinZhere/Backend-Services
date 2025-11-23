import React, { useRef } from 'react';
import { motion, useScroll, useTransform } from 'framer-motion';

import user_icon from '../../assets/user_time_line.svg';
import heart_icon from '../../assets/risk_score_timeline.svg';
import elect_icon from '../../assets/initial Assessment_timeline.svg';
import life_style_icon from '../../assets/life_style_timeline.svg';
import pepoles_icon from '../../assets/conecticon_people_timeline.svg';
import houre_icon from '../../assets/arcticons_hourlyreminder_timeline.svg';
import QrCode_icon from '../../assets/proicons_qr-code_timeline.svg';
import calendar_icon from '../../assets/calendar_timeline.svg';
import docmintion_icon from '../../assets/docmintion_timeline.svg';

const COLORS = {
  PRIMARY: "#333CF5",
  SECONDARY: "#0891B2",
  RED: "#DC2626",
  EMERALD: "#059669",
  AMBER: "#D97706",
  GRAY_BG: "#E5E7EB",
  CARD_GRADIENT_START: "#FF0000",
  CARD_GRADIENT_END: "#2564EB",
};

const TIMELINE_STEPS = [
  { 
    title: "Sign Up & Profile", 
    tag: "5 min", 
    icon: user_icon, 
    desc: "Create your account and complete your comprehensive health profile with medical history, current medications, and lifestyle factors.",
    side: "left",
    iconColor: COLORS.PRIMARY
  },
  { 
    title: "Initial Assessment", 
    tag: "2 min", 
    icon: elect_icon, 
    desc: "Our AI analyzes your data to establish baseline metrics and identify potential risk factors for personalized monitoring.",
    side: "right",
    iconColor: COLORS.SECONDARY
  },
  { 
    title: "Risk Score Calculation", 
    tag: "Instant", 
    icon: heart_icon, 
    desc: "Receive your personalized heart risk score based on advanced algorithms analyzing over 50 health parameters.",
    side: "left",
    iconColor: COLORS.RED
  },
  { 
    title: "Lifestyle Recommendations", 
    tag: "Ongoing", 
    icon: life_style_icon, 
    desc: "Get personalized diet, exercise, and lifestyle recommendations tailored to your specific risk profile and health goals.",
    side: "right",
    iconColor: COLORS.EMERALD
  },
  { 
    title: "Emergency QR Setup", 
    tag: "1 min", 
    icon: QrCode_icon, 
    desc: "Generate your emergency QR code containing critical medical information for first responders and emergency situations.",
    side: "left",
    iconColor: COLORS.RED
  },
  { 
    title: "Doctor Connection", 
    tag: "24/7", 
    icon: pepoles_icon, 
    desc: "Connect with certified cardiologists for virtual consultations, expert advice, and professional medical guidance.",
    side: "right",
    iconColor: COLORS.AMBER
  },
  { 
    title: "Smart Reminders", 
    tag: "Custom", 
    icon: houre_icon, 
    desc: "Receive intelligent reminders for medications, appointments, vital checks, and lifestyle activities to maintain optimal health.",
    side: "left",
    iconColor: COLORS.SECONDARY
  },
  { 
     title: "Health Calendar", 
    tag: "Organized", 
    icon: calendar_icon, 
    desc: "Keep track of your appointments, medication schedules, and health milestones in one integrated calendar view.",
    side: "right",
    iconColor: COLORS.PRIMARY
  
  },
  { 
    title: "Progress Reports", 
    tag: "Monthly", 
    icon: docmintion_icon, 
    desc: "Access detailed health reports, trend analysis, and progress tracking to visualize your heart health journey over time.",
    side: "left",
    iconColor: COLORS.EMERALD
  },
];

const TimelineItem = ({ step, isFirst }) => {
  const isLeft = step.side === 'left';
  const itemRef = useRef(null);

  const { scrollYProgress: cardScrollYProgress } = useScroll({
    target: itemRef,
    offset: ["start center", "center center"] 
  });

  const progressBarWidth = useTransform(cardScrollYProgress, [0.2, 0.9], ["0%", "100%"]);
  
  const gradientStyle = {
    backgroundImage: `linear-gradient(to right, ${COLORS.CARD_GRADIENT_START}, ${COLORS.CARD_GRADIENT_END})`,
  };

  const iconVariants = {
    initial: { 
      backgroundColor: COLORS.GRAY_BG,
      scale: 0.8,
      opacity: 0.5,
    },
    visible: { 
      backgroundColor: step.iconColor,
      scale: 1,
      opacity: 1,
      transition: { duration: 0.4 }
    }
  };

  return (
    <div
      ref={itemRef}
      className={`relative flex flex-col md:flex-row items-start justify-between w-full ${isFirst ? 'mb-12' : 'mb-[120px]'} last:mb-0 ${isLeft ? '' : 'md:flex-row-reverse'}`}
    >
      <motion.div
        initial={{ opacity: 0, x: isLeft ? -50 : 50 }}
        whileInView={{ opacity: 1, x: 0 }}
        viewport={{ once: true, margin: isFirst ? "-200px" : "-100px" }}
        transition={{ duration: 0.6, ease: "easeOut" }}
        className="w-full md:w-[46%]"
      >
        <div className="relative bg-white p-6 rounded-3xl shadow-[0px_4px_12px_0px_rgba(0,0,0,0.12)] outline outline-[0.20px] outline-offset-[-0.20px] outline-neutral-500 overflow-hidden">
          
          <div className="flex justify-between items-center mb-3">
             <h3 className="text-lg font-bold text-[#1C1C1E] font-['Inter'] whitespace-nowrap">
              {step.title}
            </h3>
            <div className="px-2 py-1 bg-neutral-500/20 rounded-lg flex justify-center items-center">
               <span className="text-sm font-normal text-[#1C1C1E] font-['Inter']">{step.tag}</span>
            </div>
          </div>

          <p className="text-base text-neutral-500 font-normal font-['Inter'] leading-6 mb-8">
            {step.desc}
          </p>

          <div className="absolute bottom-6 left-6 right-6 h-2.5 bg-gray-100 rounded-[10px] overflow-hidden">
            <div className="absolute w-full h-full bg-[#E5E7EB] rounded-[10px]" />
            <motion.div
              style={{ width: progressBarWidth, ...gradientStyle }}
              className="absolute h-full rounded-[10px]"
            />
          </div>
        </div>
      </motion.div>

      <div className="hidden md:block absolute left-1/2 top-0 z-10 transform -translate-x-1/2">
        {!isFirst && (
            <motion.div
                variants={iconVariants}
                initial="initial"
                whileInView="visible"
                viewport={{ once: true, margin: "-50% 0px -50% 0px" }}
                className="w-11 h-11 rounded-full flex items-center justify-center shadow-md border-[4px] border-white relative -top-[60px]" 
                style={{ backgroundColor: step.iconColor }}
            >
              <img 
                  src={step.icon} 
                  alt={step.title} 
                  className="w-6 h-6" 
              />
            </motion.div>
        )}
      </div>

      <div className="w-full md:w-[46%]"></div>
    </div>
  );
};

const JourneyTimeline = () => {
  const containerRef = useRef(null);

  const { scrollYProgress } = useScroll({
    target: containerRef,
    offset: ["start start", "end 0.6"]
  });

  const lineHeight = useTransform(scrollYProgress, [0, 1], ["0%", "100%"]);
  const TOP_ICON_COLOR = COLORS.PRIMARY;
  const FirstIcon = TIMELINE_STEPS[0].icon; 

  return (
    <section id="about" ref={containerRef} className="py-32 bg-white overflow-hidden">
      <div className="max-w-[1160px] px-4 md:px-20 mx-auto"> 
        <div className="text-center mb-20">
          <h2 className="text-4xl font-bold text-gray-900 font-['Inter'] mb-4">Your Heart Health Journey</h2>
          <p className="text-lg text-neutral-500 font-normal font-['Inter']">A comprehensive pathway to better cardiovascular health with AI-powered guidance</p>
        </div>

        <div className="relative max-w-[1160px] mx-auto mt-20">

          <div className="absolute left-1/2 transform -translate-x-1/2 top-[22px] bottom-0 w-1 bg-neutral-300 rounded-full hidden md:block overflow-hidden">
            <motion.div
              style={{ 
                height: lineHeight,
                backgroundColor: TOP_ICON_COLOR
              }}
              className="w-full rounded-full origin-top" 
            />
          </div>

          <motion.div
            initial={{ opacity: 0, scale: 0.8 }}
            animate={{ opacity: 1, scale: 1 }}
            transition={{ duration: 0.5, delay: 0.2 }}
            className="absolute left-[48%] transform -translate-x-1/2 top-[-16px] z-20 w-11 h-11 rounded-full flex items-center justify-center shadow-lg border-[4px] border-white"
            style={{ backgroundColor: TOP_ICON_COLOR }}
          >
            <img 
                src={FirstIcon} 
                alt="Sign Up Icon" 
                className="w-6 h-6" 
            />
          </motion.div>

          <div className="relative z-10 pt-16"> 
            {TIMELINE_STEPS.map((step, idx) => (
              <TimelineItem 
                key={idx} 
                step={step} 
                isFirst={idx === 0} 
              />
            ))}
          </div>

        </div>
      </div>
    </section>
  );
};

export default JourneyTimeline;