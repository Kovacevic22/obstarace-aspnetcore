import Login from "../../assets/Login.jpg";

export function LoginPage() {
    return (
        <div className="relative min-h-screen flex items-center justify-center overflow-hidden bg-[var(--color-dark)]">
            <div
                className="absolute inset-0 z-0 opacity-40"
                style={{
                    backgroundImage: `url(${Login})`,
                    backgroundSize: 'cover',
                    backgroundPosition: 'center',
                }}
            />
            <div className="relative z-10 w-full max-w-md px-6">
                <div className="bg-[var(--color-dark)]/80 backdrop-blur-xl border border-white/10 p-8 md:p-10 shadow-2xl">
                    <div className="text-center mb-10">
                        <div className="text-3xl font-black uppercase tracking-tighter text-white mb-2">
                            Athlete <span className="text-[var(--color-accent)]">Login</span>
                        </div>
                        <div className="text-[10px] text-[var(--color-light)]/50 uppercase tracking-[0.3em]">
                            Enter the arena
                        </div>
                    </div>
                    <div className="flex flex-col gap-6">
                        <div className="flex flex-col gap-2">
                            <label className="text-[10px] font-bold uppercase tracking-widest text-[var(--color-light)]/60 ml-1">
                                Email Address
                            </label>
                            <input
                                type="email"
                                className="w-full bg-white/5 border border-white/10 px-4 py-3 text-white focus:outline-none focus:border-[var(--color-accent)] transition-all placeholder:text-white/10"
                                placeholder="name@example.com"
                            />
                        </div>

                        <div className="flex flex-col gap-2">
                            <div className="flex justify-between items-center px-1">
                                <label className="text-[10px] font-bold uppercase tracking-widest text-[var(--color-light)]/60">
                                    Password
                                </label>
                                <a href="#" className="text-[9px] uppercase text-[var(--color-accent)] hover:underline">Forgot?</a>
                            </div>
                            <input
                                type="password"
                                className="w-full bg-white/5 border border-white/10 px-4 py-3 text-white focus:outline-none focus:border-[var(--color-accent)] transition-all placeholder:text-white/10"
                                placeholder="••••••••"
                            />
                        </div>
                        <button className="w-full bg-[var(--color-accent)] text-[var(--color-dark)] font-black uppercase tracking-widest py-4 mt-4 hover:bg-white transition-all shadow-lg active:scale-[0.98] cursor-pointer">
                            Sign In
                        </button>
                    </div>
                    <div className="mt-10 text-center">
                        <div className="text-xs text-[var(--color-light)]/40 uppercase tracking-widest">
                            Don't have an account?
                            <a href="#" className="ml-2 text-white font-bold hover:text-[var(--color-accent)] transition-colors">Register</a>
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