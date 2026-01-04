import Login from "../../assets/Login.jpg";
import {Link} from "react-router";
import {useState} from "react";
import {authService} from "../../services/authService.ts";
import {AxiosError} from "axios";
import * as React from "react";
import type {LoginData} from "../../Models/auth.type.ts";

export function LoginPage() {
    const [formData, setFormData] = useState<LoginData>({email:"",password:""});
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string>();
    const handleLogin = async (e: React.FormEvent) =>{
        e.preventDefault();
        setLoading(true);
        setError("");
        try{
            await authService.login(formData);
            window.location.href = "/";
        }catch(err){
            if (err instanceof AxiosError) {
                setError(err.response?.data?.error || "Login failed");
            } else {
                setError("Username or password is incorrect");
            }
        }finally {
            setLoading(false);
        }
    }
    return (
        <div className="relative min-h-screen flex items-center justify-center overflow-hidden bg-dark">
            <div
                className="absolute inset-0 z-0 opacity-40"
                style={{
                    backgroundImage: `url(${Login})`,
                    backgroundSize: 'cover',
                    backgroundPosition: 'center',
                }}
            />
            <div className="relative z-10 w-full max-w-md px-6">
                <div className="bg-dark/80 backdrop-blur-xl border border-white/10 p-8 md:p-10 shadow-2xl">
                    <div className="text-center mb-10">
                        <div className="text-3xl font-black uppercase tracking-tighter text-white mb-2">
                            Athlete <span className="text-accent">Login</span>
                        </div>
                        <div className="text-[10px] text-light/50 uppercase tracking-[0.3em]">
                            Enter the arena
                        </div>
                    </div>
                    {error && (
                        <div className="mb-6 p-3 bg-red-500/10 border border-red-500/30 text-red-500 text-[10px] uppercase font-black tracking-widest text-center">
                            {error}
                        </div>
                    )}
                    <form onSubmit={handleLogin} className="flex flex-col gap-6">
                        <div className="flex flex-col gap-2">
                            <label className="text-[10px] font-bold uppercase tracking-widest text-light/60 ml-1">
                                Email Address
                            </label>
                            <input
                                type="email"
                                className="w-full bg-white/5 border border-white/10 px-4 py-3 text-white focus:outline-none focus:border-accent transition-all placeholder:text-white/10"
                                placeholder="name@example.com"
                                required
                                value={formData.email}
                                onChange={e => setFormData({...formData, email:e.target.value})}
                            />
                        </div>

                        <div className="flex flex-col gap-2">
                            <div className="flex justify-between items-center px-1">
                                <label className="text-[10px] font-bold uppercase tracking-widest text-light/60">
                                    Password
                                </label>
                                <a href="#" className="text-[9px] uppercase text-accent hover:underline">Forgot?</a>
                            </div>
                            <input
                                type="password"
                                className="w-full bg-white/5 border border-white/10 px-4 py-3 text-white focus:outline-none focus:border-accent transition-all placeholder:text-white/10"
                                placeholder="••••••••"
                                required
                                value={formData.password}
                                onChange={e => setFormData({...formData, password:e.target.value})}
                            />
                        </div>
                        <button
                            className="w-full bg-accent text-dark font-black uppercase tracking-widest py-4 mt-4 hover:bg-white transition-all shadow-lg active:scale-[0.98] cursor-pointer"
                            type="submit"
                            disabled={loading}
                        >
                            {loading ? "Verifying..." : "Sign In"}
                        </button>
                    </form>
                    <div className="mt-10 text-center">
                        <div className="text-xs text-light/40 uppercase tracking-widest">
                            Don't have an account?
                            <Link to={"/register"} className="ml-2 text-white font-bold hover:text-accent transition-colors">Register</Link>
                        </div>
                    </div>

                </div>
                <div className="mt-8 flex justify-center gap-6 opacity-20 text-[10px] font-bold uppercase tracking-[0.2em] text-white">
                    <div className="cursor-pointer hover:opacity-100">Terms</div>
                    <div className="cursor-pointer hover:opacity-100">Privacy</div>
                    <div className="cursor-pointer hover:opacity-100">Support</div>
                </div>
            </div>
        </div>
    );
}

export default LoginPage;