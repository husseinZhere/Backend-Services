import React, { useState } from "react";
import { Link } from "react-router-dom";
import {
  Check,
  ArrowRight,
  ArrowLeft,
  Users,
  Calendar,
  FileText,
  ChevronDown,
  Search,
  LayoutDashboard,
  LogIn,
} from "lucide-react";
import AuthNavbar from "../components/layout/AuthNavbar";

const Register = () => {
  const [currentStep, setCurrentStep] = useState(2);

  // 🌟 VALIDATION STATE
  const [showTermsError, setShowTermsError] = useState(false);

  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
    phone: "",
    dob: "",
    gender: "",
    agreeTerms: false,
    bodyTemp: "",
    bloodSugar: "",
    height: "",
    weight: "",
    heartRate: "",
    bloodPressure: "",
    bloodCount: "",
    smoker: "",
  });

  const steps = [
    { id: 1, label: "Role Selection" },
    { id: 2, label: "Account Information" },
    { id: 3, label: "Health Information" },
    { id: 4, label: "Medical Info" },
    { id: 5, label: "Welcome" },
  ];

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: type === "checkbox" ? checked : value,
    }));
  };

  // 🌟 NEXT BUTTON VALIDATION
  const handleNext = () => {
    // Prevent going next IF terms not checked
    if (currentStep === 2 && !formData.agreeTerms) {
      setShowTermsError(true);

      setTimeout(() => {
        setShowTermsError(false);
      }, 2500);

      return;
    }

    if (currentStep < 5) setCurrentStep((prev) => prev + 1);
  };

  const handleBack = () => {
    if (currentStep > 2) setCurrentStep((prev) => prev - 1);
  };

  const CustomSelect = ({ label, name, value, options, placeholder }) => (
    <div className="space-y-2">
      <label className="ml-1 text-sm font-bold text-gray-700">
        {label} <span className="text-red-500">*</span>
      </label>
      <div className="relative">
        <select
          name={name}
          value={value}
          onChange={handleChange}
          className="h-[52px] w-full cursor-pointer rounded-full border border-gray-200 bg-white px-6 pr-10 text-sm text-gray-500 shadow-sm outline-none transition-all focus:border-primary focus:ring-4 focus:ring-primary/10"
        >
          <option value="" disabled>
            {placeholder}
          </option>

          {options.map((opt) => (
            <option key={opt} value={opt}>
              {opt}
            </option>
          ))}
        </select>

        <ChevronDown className="absolute right-5 top-1/2 h-5 w-5 -translate-y-1/2 text-gray-400" />
      </div>
    </div>
  );

  return (
    <div className="min-h-screen bg-[#F8F9FA] pb-20 font-sans">
      <AuthNavbar />

      {/* TOP STEPPER */}
      <div className="mx-auto mb-8 max-w-4xl px-4 pt-10">
        <div className="relative flex items-center justify-between px-4 md:px-10">
          <div className="absolute left-0 top-5 -z-10 h-[1px] w-full bg-gray-300" />

          {steps.map((step) => {
            const isCompleted = step.id < currentStep || currentStep === 5;
            const isActive = step.id === currentStep;

            return (
              <div
                key={step.id}
                className="z-10 flex flex-col items-center bg-[#F8F9FA] px-2"
              >
                <div
                  className={`flex h-10 w-10 items-center justify-center rounded-full border-[1.5px] text-sm font-bold transition-all ${
                    isCompleted || (isActive && step.id === 5)
                      ? "border-primary bg-primary text-white"
                      : isActive
                      ? "border-primary bg-white text-primary shadow-[0_0_0_4px_rgba(51,60,245,0.12)]"
                      : "border-gray-300 bg-white text-gray-400"
                  }`}
                >
                  {isCompleted ? <Check size={18} /> : step.id}
                </div>

                <span
                  className={`mt-2 text-[11px] font-medium ${
                    isActive || isCompleted ? "text-gray-800" : "text-gray-400"
                  }`}
                >
                  {step.label}
                </span>
              </div>
            );
          })}
        </div>

        <div className="mt-8 text-center text-xs text-gray-400">
          Complete this step to continue your heartcare journey
        </div>
      </div>

      {/* TITLE */}
      <div className="mb-8 px-4 text-center">
        {currentStep === 2 && (
          <>
            <h1 className="mb-2 text-3xl font-bold text-gray-900">
              Create Your Patient Account
            </h1>
            <p className="mx-auto max-w-xl text-sm text-gray-500">
              Join thousands of patients who trust MedConnect Portal.
            </p>
          </>
        )}
      </div>

      {/* MAIN CARD */}
      <div className="mx-auto mb-16 max-w-[800px] rounded-[32px] bg-white p-8 shadow-lg md:p-12">
        <form onSubmit={(e) => e.preventDefault()}>
          {/* STEP 2 */}
          {currentStep === 2 && (
            <div className="space-y-7">
              <div className="text-center">
                <h2 className="text-lg font-bold">Personal Information</h2>
                <p className="text-xs text-gray-400">
                  Please provide your basic contact information
                </p>
              </div>

              {/* INPUT GRID */}
              <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
                {[
                  {
                    l: "First Name",
                    n: "firstName",
                    p: "Enter your first name",
                  },
                  { l: "Last Name", n: "lastName", p: "Enter your last name" },
                  {
                    l: "Email Address",
                    n: "email",
                    p: "Enter your email",
                    t: "email",
                  },
                  {
                    l: "Password",
                    n: "password",
                    p: "Create a strong password",
                    t: "password",
                  },
                  { l: "Phone Number", n: "phone", p: "+20 1000000000" },
                ].map((field) => (
                  <div key={field.n} className="space-y-1.5">
                    <label className="text-xs font-bold ml-1">
                      {field.l} <span className="text-red-500">*</span>
                    </label>

                    <input
                      name={field.n}
                      type={field.t || "text"}
                      placeholder={field.p}
                      value={formData[field.n]}
                      onChange={handleChange}
                      className="h-[48px] w-full rounded-full border border-gray-200 px-6 text-sm outline-none focus:border-primary focus:ring-primary/10"
                    />
                  </div>
                ))}

                {/* DOB */}
                <div className="space-y-1.5">
                  <label className="text-xs font-bold ml-1">
                    Date of Birth <span className="text-red-500">*</span>
                  </label>

                  <input
                    name="dob"
                    type="date"
                    value={formData.dob}
                    onChange={handleChange}
                    className="h-[48px] w-full rounded-full border border-gray-300 px-6 text-sm text-gray-600 outline-none focus:border-primary focus:ring-primary/10"
                  />
                </div>
              </div>

              {/* GENDER (FIXED) */}
              <div className="mt-2">
                <label className="text-xs font-bold ml-1 text-[#000814]">
                  Gender <span className="text-red-500">*</span>
                </label>

                <div
                  className="flex items-center gap-[20px] mt-2"
                  style={{ width: "284px" }}
                >
                  {[
                    { value: "male", label: "Male" },
                    { value: "female", label: "Female" },
                  ].map((opt) => {
                    const selected = formData.gender === opt.value;

                    return (
                      <label
                        key={opt.value}
                        className={`flex items-center gap-2 px-6 h-10 rounded-full border cursor-pointer transition-all
                          ${
                            selected
                              ? "border-[#333CF5] bg-[#F5F6FF] text-[#000814]"
                              : "border-[#D1D5DB] text-[#000814]"
                          }`}
                      >
                        {/* Radio visual circle */}
                        <span
                          className={`h-4 w-4 rounded-full border flex items-center justify-center
                            ${
                              selected
                                ? "border-[#333CF5]"
                                : "border-[#B5B8BF]"
                            }`}
                        >
                          {selected && (
                            <span className="h-2 w-2 rounded-full bg-[#333CF5]"></span>
                          )}
                        </span>

                        {/* Hidden input */}
                        <input
                          type="radio"
                          name="gender"
                          value={opt.value}
                          checked={selected}
                          onChange={handleChange}
                          className="hidden"
                        />

                        <span
                          className={
                            selected ? "text-[#000814]" : "text-[#6B7280]"
                          }
                        >
                          {opt.label}
                        </span>
                      </label>
                    );
                  })}
                </div>
              </div>

              {/* TERMS */}
              <div className="pt-2">
                <div className="flex items-center gap-2 mb-2">
                  <FileText className="w-4 text-primary" />
                  <h3 className="text-sm font-bold">
                    Terms & Consent <span className="text-red-500">*</span>
                  </h3>
                </div>

                <label className="flex gap-3 p-2 rounded-lg cursor-pointer hover:bg-gray-50">
                  <input
                    type="checkbox"
                    name="agreeTerms"
                    checked={formData.agreeTerms}
                    onChange={handleChange}
                    className="w-4 h-4"
                  />
                  <div className="text-xs text-gray-500 leading-snug">
                    <span className="font-bold text-gray-700">
                      I agree to the Terms of Service and Privacy Policy
                    </span>
                    <br />
                    By checking this box, you agree to our terms and conditions.
                  </div>
                </label>

                {/* 🌟 TOOLTIP VALIDATION HERE */}
                {showTermsError && (
                  <div className="relative mt-2">
                    <div className="absolute left-6 top-2 z-50 flex items-center gap-2 rounded-lg border border-gray-200 bg-white px-3 py-2 text-xs text-gray-700 shadow-lg">
                      <span className="text-yellow-500 text-base">⚠</span>
                      Please check this box if you want to proceed
                      <div className="absolute -left-2 top-3 h-3 w-3 rotate-45 border-l border-t border-gray-200 bg-white"></div>
                    </div>
                  </div>
                )}
              </div>
            </div>
          )}

          {/* ================= STEP 3 ================= */}
          {currentStep === 3 && (
            <div className="space-y-8">
              <div className="text-center">
                <h2 className="text-lg font-bold">Health Information</h2>
                <p className="text-xs text-gray-400">
                  Enter your health measurements to continue
                </p>
              </div>

              <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                {[
                  { l: "Body Temperature (°C)", n: "bodyTemp", p: "37.0" },
                  { l: "Blood Sugar", n: "bloodSugar", p: "mg/dL" },
                  { l: "Height", n: "height", p: "cm" },
                  { l: "Weight", n: "weight", p: "kg" },
                ].map((field) => (
                  <div key={field.n}>
                    <label className="text-xs font-bold ml-1">
                      {field.l} <span className="text-red-500">*</span>
                    </label>
                    <input
                      name={field.n}
                      value={formData[field.n]}
                      onChange={handleChange}
                      placeholder={field.p}
                      className="h-[48px] w-full rounded-full border border-gray-300 px-6 text-sm outline-none focus:border-primary focus:ring-primary/10"
                    />
                  </div>
                ))}
              </div>
            </div>
          )}

          {/* ================= STEP 4 ================= */}
          {currentStep === 4 && (
            <div className="space-y-6 max-w-2xl mx-auto">
              <div className="text-center">
                <h2 className="text-lg font-bold">Medical Information</h2>
                <p className="text-xs text-gray-400">
                  Enter your medical details
                </p>
              </div>

              <CustomSelect
                label="Heart Rate"
                name="heartRate"
                value={formData.heartRate}
                options={["Below 60", "60-100 (Normal)", "Above 100"]}
                placeholder="Select range"
              />

              <CustomSelect
                label="Blood Pressure"
                name="bloodPressure"
                value={formData.bloodPressure}
                options={["Normal", "Elevated", "High"]}
                placeholder="Select pressure"
              />

              <CustomSelect
                label="Blood Count"
                name="bloodCount"
                value={formData.bloodCount}
                options={["Normal", "Low", "High"]}
                placeholder="Select count"
              />

              {/* Smoker */}
              <div>
                <label className="text-sm font-bold">
                  Do you smoke? <span className="text-red-500">*</span>
                </label>

                <div className="flex gap-6 mt-2">
                  {["Yes", "No"].map((opt) => (
                    <label key={opt} className="flex items-center gap-3">
                      <div
                        className={`h-6 w-6 rounded-full border flex items-center justify-center ${
                          formData.smoker === opt.toLowerCase()
                            ? "border-primary"
                            : "border-gray-300"
                        }`}
                      >
                        {formData.smoker === opt.toLowerCase() && (
                          <div className="h-3 w-3 bg-primary rounded-full"></div>
                        )}
                      </div>

                      <input
                        type="radio"
                        name="smoker"
                        value={opt.toLowerCase()}
                        checked={formData.smoker === opt.toLowerCase()}
                        onChange={handleChange}
                        className="hidden"
                      />

                      <span>{opt}</span>
                    </label>
                  ))}
                </div>
              </div>
            </div>
          )}

          {/* ================= STEP 5 ================= */}
          {currentStep === 5 && (
            <div className="text-center py-10 rounded-3xl border border-gray-200 bg-white shadow-sm px-6">
              {/* Success Icon */}
              <div className="h-20 w-20 bg-green-500 text-white rounded-full flex items-center justify-center mx-auto ring-4 ring-green-200 shadow mb-6">
                <Check size={40} />
              </div>

              <h2 className="text-3xl font-bold">
                Welcome to Pulse<span className="text-primary">X</span>!
              </h2>

              <p className="text-gray-500 mt-3">
                Hi <span className="font-bold">{formData.firstName || "User"}</span>
                , your patient account has been successfully created.
              </p>

              {/* 🌟 Container Around Cards */}
              <div className="mt-10 rounded-2xl bg-[#F9FAFB] p-8">
                <h3 className="text-sm font-bold text-gray-900 mb-6">
                  What's Next?
                </h3>

                {/* Cards Grid */}
                <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                  {/* Card 1 */}
                  <div className="rounded-2xl border border-gray-200 bg-white p-6 shadow-sm hover:shadow-md transition cursor-pointer">
                    <div className="mx-auto mb-3 flex h-12 w-12 items-center justify-center rounded-full bg-blue-100 text-blue-600">
                      <Search size={22} />
                    </div>
                    <h4 className="font-bold text-gray-900">Find Doctors</h4>
                    <p className="text-xs text-gray-500 mt-1">
                      Browse our network of healthcare professionals
                    </p>
                  </div>

                  {/* Card 2 */}
                  <div className="rounded-2xl border border-gray-200 bg-white p-6 shadow-sm hover:shadow-md transition cursor-pointer">
                    <div className="mx-auto mb-3 flex h-12 w-12 items-center justify-center rounded-full bg-green-100 text-green-600">
                      <Calendar size={22} />
                    </div>
                    <h4 className="font-bold text-gray-900">
                      Book Appointment
                    </h4>
                    <p className="text-xs text-gray-500 mt-1">
                      Schedule your first consultation
                    </p>
                  </div>

                  {/* Card 3 */}
                  <div className="rounded-2xl border border-gray-200 bg-white p-6 shadow-sm hover:shadow-md transition cursor-pointer">
                    <div className="mx-auto mb-3 flex h-12 w-12 items-center justify-center rounded-full bg-orange-100 text-orange-500">
                      <FileText size={22} />
                    </div>
                    <h4 className="font-bold text-gray-900">Medical Records</h4>
                    <p className="text-xs text-gray-500 mt-1">
                      Access and manage your health information
                    </p>
                  </div>
                </div>
              </div>

              {/* Continue Button */}
              <Link to="/" className="block mt-10">
                <button className="w-full max-w-xs mx-auto bg-primary text-white font-bold py-3 rounded-full shadow-md hover:bg-primary-hover flex items-center justify-center gap-2">
                  Continue to Dashboard <LayoutDashboard size={18} />
                </button>
              </Link>
            </div>
          )}

          {/* ================= NAVIGATION CONTROLS ================= */}
          {currentStep < 5 && (
            <div className="mt-12 border-t border-gray-200 pt-6">
              <div className="flex items-center justify-between">
                {/* Previous */}
                <button
                  type="button"
                  onClick={handleBack}
                  disabled={currentStep === 2}
                  className={`flex items-center gap-2 px-3 py-2 text-sm font-bold ${
                    currentStep === 2
                      ? "text-gray-300 cursor-not-allowed"
                      : "text-gray-600 hover:text-gray-800"
                  }`}
                >
                  <ArrowLeft size={18} />
                  Previous
                </button>

                {/* Pagination Dots */}
                <div className="flex gap-2">
                  {[2, 3, 4, 5].map((dotStep) => (
                    <div
                      key={dotStep}
                      className={`rounded-full transition-all duration-300 ${
                        currentStep === dotStep
                          ? "w-3 h-3 bg-primary"
                          : "w-2 h-2 bg-gray-300"
                      }`}
                    ></div>
                  ))}
                </div>

                {/* Next */}
                <button
                  type="button"
                  onClick={handleNext}
                  className="flex items-center gap-2 bg-[#333CF5] hover:bg-[#2D32D8] text-white px-8 py-3 rounded-[24px] shadow-md transition-all"
                >
                  Next
                  <ArrowRight size={18} />
                </button>
              </div>
            </div>
          )}
        </form>

        {/* ================= ALREADY HAVE ACCOUNT BOX ================= */}
        {currentStep === 2 && (
          <div className="mt-12 rounded-2xl border border-gray-300 bg-[#F3F5F8] p-6 shadow-md text-center">
            <div className="flex items-center justify-center gap-2 text-gray-800 font-bold">
              <Users size={18} className="text-primary" />
              Already have an account?
            </div>

            <p className="text-xs text-gray-500 mt-2">
              If you’re an existing patient, sign in to access your medical
              records and appointments.
            </p>

            <Link to="/login" className="block mt-4">
              <button className="w-full max-w-xs mx-auto bg-[#00C853] hover:bg-[#00B44A] text-white font-bold py-3 px-6 rounded-lg shadow-md flex items-center justify-center gap-2 transition-all">
                Sign In to Existing Account
                <LogIn size={18} />
              </button>
            </Link>
          </div>
        )}
      </div>

      {/* ================= FOOTER FEATURES ================= */}
      <div className="mx-auto grid max-w-4xl grid-cols-1 md:grid-cols-3 gap-10 text-center px-4">
        {[
          {
            icon: Users,
            color: "blue",
            title: "Expert Care Team",
            desc: "Connect with board-certified physicians and healthcare specialists.",
          },
          {
            icon: Calendar,
            color: "green",
            title: "Easy Scheduling",
            desc: "Book appointments 24/7 with our convenient online scheduling system.",
          },
          {
            icon: FileText,
            color: "orange",
            title: "Digital Records",
            desc: "Access your medical history and test results anytime, anywhere.",
          },
        ].map((feat, idx) => (
          <div key={idx} className="flex flex-col items-center gap-2">
            <div
              className={`h-12 w-12 rounded-full flex items-center justify-center ${
                feat.color === "blue"
                  ? "bg-blue-100 text-blue-600"
                  : feat.color === "green"
                  ? "bg-green-100 text-green-600"
                  : "bg-orange-100 text-orange-500"
              }`}
            >
              <feat.icon size={20} />
            </div>

            <h4 className="font-bold text-gray-900 text-sm">{feat.title}</h4>

            <p className="text-[11px] text-gray-500 max-w-[200px] leading-relaxed">
              {feat.desc}
            </p>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Register;