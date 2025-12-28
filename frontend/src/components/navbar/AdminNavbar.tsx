import LogoImg from "../../assets/Logo.png"
export function AdminNavbar(){
    return(
        <div className="fixed top-0 left-0 right-0 z-50 bg-[var(--color-dark)]/95 backdrop-blur-md border-b border-[var(--color-accent)]/20">
            <div className="max-w-screen-2xl mx-auto px-6 h-16 flex justify-between items-center">
                <div className="flex items-center gap-3 group cursor-pointer">
                    <div className="relative w-10 h-10 flex items-center justify-center">
                        <div className="absolute inset-0 bg-[var(--color-accent)] opacity-0 group-hover:opacity-20 blur-lg transition-all duration-500 rounded-full"></div>
                        <img src={LogoImg} alt="Logo" className="relative z-10 w-full h-full object-contain" />
                    </div>
                    <div className="text-white font-black text-sm uppercase tracking-tighter">
                        Admin<span className="text-[var(--color-accent)]">Panel</span>
                    </div>
                </div>
                <ul className="hidden xl:flex items-center gap-6 text-[10px] font-bold uppercase tracking-widest text-white/70">
                    <li><a href="#" className="hover:text-[var(--color-accent)] transition-all">Dashboard</a></li>
                    <li><a href="#" className="hover:text-[var(--color-accent)] transition-all">Manage Races</a></li>
                    <li><a href="#" className="hover:text-[var(--color-accent)] transition-all">Manage Obstacles</a></li>
                    <li><a href="#" className="hover:text-[var(--color-accent)] transition-all">Manage Users</a></li>
                    <li><a href="#" className="hover:text-[var(--color-accent)] transition-all">Registrations</a></li>
                </ul>
                <div className="px-4 py-1.5 border border-red-500/40 text-red-500 text-[10px] font-black uppercase tracking-widest hover:bg-red-500 hover:text-white cursor-pointer transition-all active:scale-95">
                    Logout
                </div>
            </div>
        </div>
    )
}
export default AdminNavbar;