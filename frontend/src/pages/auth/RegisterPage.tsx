import Register from "../../assets/Register.jpg";

export function RegisterPage() {
    return (
        <div className="relative min-h-screen flex items-center justify-center py-12 px-4 bg-[var(--color-dark)]">

            <div
                className="absolute inset-0 z-0 opacity-30"
                style={{
                    backgroundImage: `url(${Register})`,
                    backgroundSize: 'cover',
                    backgroundPosition: 'center',
                }}
            />

            <div className="relative z-10 w-full max-w-2xl">
                <div className="bg-[var(--color-dark)]/90 backdrop-blur-xl border border-white/10 p-8 md:p-12 shadow-2xl">


                    <div className="text-center mb-10">
                        <div className="text-3xl font-black uppercase tracking-tighter text-white">
                            Create <span className="text-[var(--color-accent)]">Account</span>
                        </div>
                        <div className="text-[10px] text-[var(--color-light)]/50 uppercase tracking-[0.3em] mt-2">
                            Join the elite OCR community
                        </div>
                    </div>

                    <form className="flex flex-col gap-5">

                        <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
                            <div className="flex flex-col gap-1.5">
                                <label className="text-[10px] font-bold uppercase tracking-widest text-[var(--color-light)]/60 ml-1">First Name</label>
                                <input type="text" className="bg-white/5 border border-white/10 px-4 py-2.5 text-white focus:outline-none focus:border-[var(--color-accent)] transition-all" placeholder="John" required />
                            </div>
                            <div className="flex flex-col gap-1.5">
                                <label className="text-[10px] font-bold uppercase tracking-widest text-[var(--color-light)]/60 ml-1">Surname</label>
                                <input type="text" className="bg-white/5 border border-white/10 px-4 py-2.5 text-white focus:outline-none focus:border-[var(--color-accent)] transition-all" placeholder="Doe" required />
                            </div>
                        </div>

                        <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
                            <div className="flex flex-col gap-1.5">
                                <label className="text-[10px] font-bold uppercase tracking-widest text-[var(--color-light)]/60 ml-1">Email Address</label>
                                <input type="email" className="bg-white/5 border border-white/10 px-4 py-2.5 text-white focus:outline-none focus:border-[var(--color-accent)] transition-all" placeholder="john@example.com" required />
                            </div>
                            <div className="flex flex-col gap-1.5">
                                <label className="text-[10px] font-bold uppercase tracking-widest text-[var(--color-light)]/60 ml-1">Phone Number</label>
                                <input type="tel" className="bg-white/5 border border-white/10 px-4 py-2.5 text-white focus:outline-none focus:border-[var(--color-accent)] transition-all" placeholder="+381..." required />
                            </div>
                        </div>

                        <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
                            <div className="flex flex-col gap-1.5">
                                <label className="text-[10px] font-bold uppercase tracking-widest text-[var(--color-light)]/60 ml-1">Date of Birth</label>
                                <input type="date" className="bg-white/5 border border-white/10 px-4 py-2.5 text-white focus:outline-none focus:border-[var(--color-accent)] transition-all appearance-none" required />
                            </div>
                            <div className="flex flex-col gap-1.5">
                                <label className="text-[10px] font-bold uppercase tracking-widest text-[var(--color-accent)] ml-1 italic">Emergency Contact</label>
                                <input type="tel" className="bg-white/5 border border-[var(--color-accent)]/30 px-4 py-2.5 text-white focus:outline-none focus:border-[var(--color-accent)] transition-all" placeholder="ICE Phone Number" required />
                            </div>
                        </div>

                        <div className="flex flex-col gap-1.5">
                            <label className="text-[10px] font-bold uppercase tracking-widest text-[var(--color-light)]/60 ml-1">Password (Min 8 characters)</label>
                            <input type="password" minLength={8} className="bg-white/5 border border-white/10 px-4 py-2.5 text-white focus:outline-none focus:border-[var(--color-accent)] transition-all" placeholder="••••••••" required />
                        </div>

                        <button className="w-full bg-[var(--color-accent)] text-[var(--color-dark)] font-black uppercase tracking-widest py-4 mt-6 hover:bg-white transition-all shadow-lg active:scale-[0.98] cursor-pointer">
                            Complete Registration
                        </button>
                    </form>

                    <div className="mt-8 text-center">
                        <div className="text-[10px] text-[var(--color-light)]/40 uppercase tracking-widest">
                            Already a member?
                            <a href="#" className="ml-2 text-white font-bold hover:text-[var(--color-accent)] transition-colors underline decoration-[var(--color-accent)] underline-offset-4">Sign In</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default RegisterPage;