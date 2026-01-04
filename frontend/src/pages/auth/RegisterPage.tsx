import Register from "../../assets/Register.jpg";
import {Link} from "react-router";
import {useState} from "react";
import {authService, type RegisterData} from "../../services/authService.ts";
import * as React from "react";
import {AxiosError} from "axios";

export function RegisterPage() {
    const [formData, setFormData] = useState<RegisterData>({email:"",password:"",name:"",surname:"",phoneNumber:"",emergencyContact:"",dateOfBirth:""});
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string>();
    const maxDate = new Date();
    maxDate.setFullYear(maxDate.getFullYear() - 18);
    const dateLimit = maxDate.toISOString().split("T")[0];
    const handleRegister = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setError("");
        try{
            await authService.register(formData);
            window.location.href = "/login";
        }catch(err){
            console.log(err);
            if(err instanceof AxiosError){
                setError(err.response?.data?.error || "Register failed");
            }else {
                setError("E-mail is already registered");
            }
        }finally {
            setLoading(false);
        }
    }
    return (
        <div className="relative min-h-screen flex items-center justify-center py-12 px-4 bg-dark">

            <div
                className="absolute inset-0 z-0 opacity-30"
                style={{
                    backgroundImage: `url(${Register})`,
                    backgroundSize: 'cover',
                    backgroundPosition: 'center',
                }}
            />

            <div className="relative z-10 w-full max-w-2xl">
                <div className="bg-dark/90 backdrop-blur-xl border border-white/10 p-8 md:p-12 shadow-2xl">


                    <div className="text-center mb-10">
                        <div className="text-3xl font-black uppercase tracking-tighter text-white">
                            Create <span className="text-accent">Account</span>
                        </div>
                        <div className="text-[10px] text-light/50 uppercase tracking-[0.3em] mt-2">
                            Join the elite OCR community
                        </div>
                    </div>
                    {error && (
                        <div className="mb-6 p-3 bg-red-500/10 border border-red-500/30 text-red-500 text-[10px] uppercase font-black tracking-widest text-center">
                            {error}
                        </div>
                    )}
                    <form onSubmit={handleRegister} className="flex flex-col gap-5">

                        <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
                            <div className="flex flex-col gap-1.5">
                                <label className="text-[10px] font-bold uppercase tracking-widest text-light/60 ml-1">First Name</label>
                                <input
                                    type="text"
                                    className="bg-white/5 border border-white/10 px-4 py-2.5 text-white focus:outline-none focus:border-accent transition-all"
                                    placeholder="John"
                                    required
                                    value={formData.name}
                                    onChange={(e) => setFormData({...formData, name:e.target.value})}
                                />
                            </div>
                            <div className="flex flex-col gap-1.5">
                                <label className="text-[10px] font-bold uppercase tracking-widest text-light/60 ml-1">Surname</label>
                                <input
                                    type="text"
                                    className="bg-white/5 border border-white/10 px-4 py-2.5 text-white focus:outline-none focus:border-accent transition-all"
                                    placeholder="Doe"
                                    required
                                    value={formData.surname}
                                    onChange={(e) => setFormData({...formData, surname:e.target.value})}
                                />
                            </div>
                        </div>

                        <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
                            <div className="flex flex-col gap-1.5">
                                <label className="text-[10px] font-bold uppercase tracking-widest text-light/60 ml-1">Email Address</label>
                                <input
                                    type="email"
                                    className="bg-white/5 border border-white/10 px-4 py-2.5 text-white focus:outline-none focus:border-accent transition-all"
                                    placeholder="john@example.com"
                                    required
                                    value={formData.email}
                                    onChange={(e) => setFormData({...formData, email:e.target.value})}
                                />
                            </div>
                            <div className="flex flex-col gap-1.5">
                                <label className="text-[10px] font-bold uppercase tracking-widest text-light/60 ml-1">Phone Number</label>
                                <input
                                    type="tel"
                                    className="bg-white/5 border border-white/10 px-4 py-2.5 text-white focus:outline-none focus:border-accent transition-all"
                                    placeholder="+381..."
                                    required
                                    value={formData.phoneNumber}
                                    onChange={(e) => setFormData({...formData, phoneNumber:e.target.value})}
                                />
                            </div>
                        </div>

                        <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
                            <div className="flex flex-col gap-1.5">
                                <label className="text-[10px] font-bold uppercase tracking-widest text-light/60 ml-1">Date of Birth</label>
                                <input
                                    type="date"
                                    className="bg-white/5 border border-white/10 px-4 py-2.5 text-white focus:outline-none focus:border-accent transition-all appearance-none"
                                    required
                                    max={dateLimit}
                                    value={formData.dateOfBirth}
                                    onChange={(e) => setFormData({...formData, dateOfBirth:e.target.value})}
                                />
                            </div>
                            <div className="flex flex-col gap-1.5">
                                <label className="text-[10px] font-bold uppercase tracking-widest text-accent ml-1 italic">Emergency Contact</label>
                                <input
                                    type="tel"
                                    className="bg-white/5 border border-accent/30 px-4 py-2.5 text-white focus:outline-none focus:border-accent transition-all"
                                    placeholder="ICE Phone Number"
                                    required
                                    value={formData.emergencyContact}
                                    onChange={(e) => setFormData({...formData, emergencyContact:e.target.value})}
                                />
                            </div>
                        </div>

                        <div className="flex flex-col gap-1.5">
                            <label className="text-[10px] font-bold uppercase tracking-widest text-light/60 ml-1">Password (Min 8 characters)</label>
                            <input
                                type="password"
                                minLength={8}
                                className="bg-white/5 border border-white/10 px-4 py-2.5 text-white focus:outline-none focus:border-accent transition-all"
                                placeholder="••••••••"
                                required
                                value={formData.password}
                                onChange={(e) => setFormData({...formData, password:e.target.value})}
                            />
                        </div>

                        <button className="w-full bg-accent text-dark font-black uppercase tracking-widest py-4 mt-6 hover:bg-white transition-all shadow-lg active:scale-[0.98] cursor-pointer">
                            {loading ? "Verifying..." : "Complete registration"}
                        </button>
                    </form>

                    <div className="mt-8 text-center">
                        <div className="text-[10px] text-light/40 uppercase tracking-widest">
                            Already a member?
                            <Link to={"/login"} className="ml-2 text-white font-bold hover:text-accent transition-colors underline decoration-accent underline-offset-4">Sign In</Link>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default RegisterPage;