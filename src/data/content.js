
import { Heart, Activity, Shield, Video, FileText, QrCode, Bell, User, Calendar } from 'lucide-react';

export const BRAND = {
  name: "PulseX",
  highlight: "X"
};

export const NAV_LINKS = [
  { label: 'Home', href: '#home' },
  { label: 'Doctors', href: '#doctors' },       
  { label: 'Features', href: '#features' }, 
  { label: 'Stories', href: '#stories' },
  { label: 'About', href: '#about' },
];

export const HERO_CONTENT = {
  title: "Pulse",
  highlight: "X",
  subtitle: "AI-powered cardiovascular health monitoring with real-time risk assessment and remote doctor consultations.",
  features: [
    "AI Heart Risk Score with 95% accuracy",
    "24/7 vital signs monitoring",
    "Emergency QR codes for instant access",
    "Remote doctor follow-ups"
  ]
};

export const FEATURES_DATA = [
  { title: "AI Heart Risk Score", icon: Heart, desc: "Advanced algorithms analyze vital signs with 95% accuracy." },
  { title: "24/7 Monitoring", icon: Activity, desc: "Continuous tracking of your heart health metrics." },
  { title: "Emergency QR", icon: QrCode, desc: "Instant medical access for first responders." },
  { title: "Remote Consults", icon: Video, desc: "Virtual appointments with top specialists." },
  { title: "Health Reports", icon: FileText, desc: "Detailed monthly progress reports." },
];

export const TIMELINE_STEPS = [
  { title: "Sign Up & Profile", time: "5 min", icon: User, side: "left" },
  { title: "Initial Assessment", time: "2 min", icon: Activity, side: "right" },
  { title: "Risk Score Calculation", time: "Instant", icon: Shield, side: "left" },
  { title: "Lifestyle Recommendations", time: "Ongoing", icon: Calendar, side: "right" },
  { title: "Doctor Connection", time: "24/7", icon: Video, side: "left" },
  { title: "Smart Reminders", time: "Custom", icon: Bell, side: "right" },
];